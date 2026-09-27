using FlowIoC.BaseModule.Connectors;
using FlowIoC.BaseModule.Contexts;
using Modules.GridModule.Signals;
using Modules.InputModule.Signals;

namespace Modules.ConnectorModule.RootsContexts
{
    /// <summary>What the grid is told: every press of the pointer that started on the world, so it can say what the press landed on.</summary>
    public class GridConnectorSubContext : Context
    {
        private GridSignals _gridSignals;
        private InputSignals _inputSignals;

        public override void Setup()
        {
            base.Setup();

            _gridSignals = InjectionBinderCrossContext.GetInstance<GridSignals>();
            _inputSignals = InjectionBinderCrossContext.GetInstance<InputSignals>();

            IncomingSignals();
        }

        private void IncomingSignals() =>
            _inputSignals.Outgoing.PointerPressed.Connect(_gridSignals.Incoming.PointerPressed);

        public override void DestroyContext()
        {
            UnbindIncomingSignals();

            base.DestroyContext();
        }

        private void UnbindIncomingSignals() => _inputSignals.Outgoing.PointerPressed.Disconnect();
    }
}
