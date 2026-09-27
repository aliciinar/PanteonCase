using FlowIoC.BaseModule.Contexts;
using Modules.GridModule.Controllers;
using Modules.GridModule.Models;
using Modules.GridModule.Services;
using Modules.GridModule.Signals;

namespace Modules.GridModule.RootsContexts
{
    public class GridSystemContext : Context
    {
        private GridSignals _signals;

        public override void SignalBindings()
        {
            base.SignalBindings();
            _signals = InjectionBinderCrossContext.Bind<GridSignals>();
        }

        public override void InjectionBindings()
        {
            base.InjectionBindings();

            InjectionBinder.Bind<IGridModel, GridModel>();

            // Every module that acts on the board asks it through this interface.
            InjectionBinderCrossContext.Bind<IGridService, GridService>();
        }

        public override void CommandBindings()
        {
            base.CommandBindings();

            // A press on the board is answered with what it landed on: a building, a unit, or nothing. A secondary
            // press is an order: to go to a free cell, or to attack what stands on it.
            CommandBinder.Bind(_signals.Incoming.PointerPressed).ToSequence<PickCellCommand>();
            CommandBinder.Bind(_signals.Incoming.PointerSecondaryPressed).ToSequence<PickSecondaryCellCommand>();

            // A strike lowers what it hit; what reaches 0 leaves the board.
            CommandBinder.Bind(_signals.Incoming.DamageOccupant).ToSequence<DamageOccupantCommand>();

            // While a building is being placed a press only moves it, so nothing is picked.
            CommandBinder.Bind(_signals.Incoming.SuspendPicking).ToSequence<SetPickingCommand>(false);
            CommandBinder.Bind(_signals.Incoming.ResumePicking).ToSequence<SetPickingCommand>(true);
        }
    }
}
