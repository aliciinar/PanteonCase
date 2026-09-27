using FlowIoC.BaseModule.Contexts;
using FlowIoC.BaseModule.Controller.Commands;
using Modules.UnitsModule.Controllers;
using Modules.UnitsModule.Controllers.BoardUnits;
using Modules.UnitsModule.Models;
using Modules.UnitsModule.Signals;

namespace Modules.UnitsModule.RootsContexts
{
    public class UnitsSystemContext : Context
    {
        private UnitsSignals _signals;
        private UnitsInternalSignals _internalSignals;

        public override void SignalBindings()
        {
            base.SignalBindings();
            _internalSignals = InjectionBinder.Bind<UnitsInternalSignals>();
            _signals = InjectionBinderCrossContext.Bind<UnitsSignals>();
        }

        public override void InjectionBindings()
        {
            base.InjectionBindings();
            InjectionBinder.Bind<IUnitsModel, UnitsModel>();
            InjectionBinder.Bind<IUnitSelectionModel, UnitSelectionModel>();
        }

        public override void CommandBindings()
        {
            base.CommandBindings();

            // Every action - walking out of a door, walking to a cell, walking up to strike - is announced as started
            // once it is set playing on the unit object, and as ended when that object's tweens are over; the game
            // waits for it. SignalDispatchCommand releases no data, so it goes last.

            // A unit asked of a building comes out of the building's exit point - its door - and walks by A*
            // to the building's spawn point, or, when that is taken or off the grid, the free cell nearest it.
            CommandBinder.Bind(_signals.Incoming.SpawnUnit)
                .ToSequence<FindUnitGoalCommand>()
                .ToSequence<PlanUnitPathCommand>()
                .ToSequence<PlaceBoardUnitCommand>()
                .ToSequence<SignalDispatchCommand>(_signals.Outgoing.ActionStarted);

            // A pressed unit is selected and tinted; a press anywhere else leaves no unit selected.
            CommandBinder.Bind(_signals.Incoming.SelectUnit).ToSequence<SelectUnitCommand>();
            CommandBinder.Bind(_signals.Incoming.ClearSelection).ToSequence<ClearUnitSelectionCommand>();

            // A free cell ordered with the selected unit: it takes the cell and walks there by A*, around buildings.
            CommandBinder.Bind(_signals.Incoming.MoveSelectedUnit)
                .ToSequence<PlanUnitMoveCommand>()
                .ToSequence<WalkBoardUnitCommand>()
                .ToSequence<SignalDispatchCommand>(_signals.Outgoing.ActionStarted);

            // A building or a unit ordered attacked with the selected unit: it walks by A* to the free cell next to
            // the target nearest it and strikes once.
            CommandBinder.Bind(_signals.Incoming.AttackWithSelectedUnit)
                .ToSequence<PlanAttackCommand>()
                .ToSequence<StrikeWithBoardUnitCommand>()
                .ToSequence<SignalDispatchCommand>(_signals.Outgoing.ActionStarted);

            CommandBinder.Bind(_internalSignals.UnitStruck).ToSequence<StrikeCommand>();
            CommandBinder.Bind(_internalSignals.UnitActionFinished).ToSequence<SignalDispatchCommand>(_signals.Outgoing.ActionEnded);

            // A struck unit shows the hit; a destroyed one - already off the board - is no longer selected and its
            // object goes back to the pool.
            CommandBinder.Bind(_signals.Incoming.UnitDamaged).ToSequence<ShowBoardUnitHitCommand>();
            CommandBinder.Bind(_signals.Incoming.RemoveUnit).ToSequence<RemoveBoardUnitCommand>();
        }
    }
}
