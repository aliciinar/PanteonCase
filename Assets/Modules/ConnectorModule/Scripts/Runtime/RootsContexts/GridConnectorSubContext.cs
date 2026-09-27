using FlowIoC.BaseModule.Connectors;
using FlowIoC.BaseModule.Contexts;
using Modules.BuildingsModule.Signals;
using Modules.GridModule.Signals;
using Modules.InputModule.Signals;

namespace Modules.ConnectorModule.RootsContexts
{
    /// <summary>
    /// What the grid is told: every press and secondary press of the pointer that started on the world, so it can say
    /// what the press landed on - and, while a building is being placed, that presses belong to the placement.
    /// </summary>
    public class GridConnectorSubContext : Context
    {
        private GridSignals _gridSignals;
        private InputSignals _inputSignals;
        private BuildingsSignals _buildingsSignals;

        public override void Setup()
        {
            base.Setup();

            _gridSignals = InjectionBinderCrossContext.GetInstance<GridSignals>();
            _inputSignals = InjectionBinderCrossContext.GetInstance<InputSignals>();
            _buildingsSignals = InjectionBinderCrossContext.GetInstance<BuildingsSignals>();

            IncomingSignals();
        }

        private void IncomingSignals()
        {
            _inputSignals.Outgoing.PointerPressed.Connect(_gridSignals.Incoming.PointerPressed);
            _inputSignals.Outgoing.PointerSecondaryPressed.Connect(_gridSignals.Incoming.PointerSecondaryPressed);

            // While a building is being placed, a press only moves it.
            _buildingsSignals.Outgoing.PlacementStarted.Connect(_gridSignals.Incoming.SuspendPicking);
            _buildingsSignals.Outgoing.PlacementEnded.Connect(_gridSignals.Incoming.ResumePicking);
        }

        public override void DestroyContext()
        {
            UnbindIncomingSignals();

            base.DestroyContext();
        }

        private void UnbindIncomingSignals()
        {
            _inputSignals.Outgoing.PointerPressed.Disconnect();
            _inputSignals.Outgoing.PointerSecondaryPressed.Disconnect();
            _buildingsSignals.Outgoing.PlacementStarted.Disconnect();
            _buildingsSignals.Outgoing.PlacementEnded.Disconnect();
        }
    }
}
