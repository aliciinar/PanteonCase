using FlowIoC.BaseModule.Contexts;
using Modules.UnitsModule.Controllers;
using Modules.UnitsModule.Models;
using Modules.UnitsModule.Signals;
using Modules.UnitsModule.ViewsMediators;

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

        public override void MediationBindings()
        {
            base.MediationBindings();
            MediationBinder.Bind<PlacedUnitsView>().To<PlacedUnitsMediator>();
        }

        public override void CommandBindings()
        {
            base.CommandBindings();

            // A unit asked of a building comes out of the building's exit point - its door - and walks by A*
            // to the building's spawn point, or, when that is taken or off the grid, the free cell nearest it.
            CommandBinder.Bind(_signals.Incoming.SpawnUnit)
                .ToSequence<FindUnitGoalCommand>()
                .ToSequence<PlanUnitPathCommand>()
                .ToSequence<ShowUnitCommand>();

            // A pressed unit is selected and tinted; a press anywhere else leaves no unit selected.
            CommandBinder.Bind(_signals.Incoming.SelectUnit).ToSequence<SelectUnitCommand>();
            CommandBinder.Bind(_signals.Incoming.ClearSelection).ToSequence<ClearUnitSelectionCommand>();

            // A free cell ordered with the selected unit: it takes the cell and walks there by A*, around buildings.
            CommandBinder.Bind(_signals.Incoming.MoveSelectedUnit)
                .ToSequence<PlanUnitMoveCommand>()
                .ToSequence<MoveUnitCommand>();

            // Every cell a walking unit steps into is kept by the grid, so a new order turns the unit where it is.
            CommandBinder.Bind(_internalSignals.UnitStepped).ToSequence<TrackUnitStepCommand>();
        }
    }
}
