using FlowIoC.BaseModule.Connectors;
using FlowIoC.BaseModule.Contexts;
using Modules.BuildingsModule.ProductionMenuScreenModule.Signals;
using Modules.BuildingsModule.Signals;
using Modules.InputModule.Signals;

namespace Modules.ConnectorModule.RootsContexts
{
    /// <summary>
    /// What the buildings module is told: which building the player picked in its production menu, and
    /// every press of the pointer that started on the world - where it began, where it was dragged,
    /// where it ended.
    /// </summary>
    public class BuildingsConnectorSubContext : Context
    {
        private BuildingsSignals _buildingsSignals;
        private ProductionMenuScreenSignals _productionMenuScreenSignals;
        private InputSignals _inputSignals;

        public override void Setup()
        {
            base.Setup();

            _buildingsSignals = InjectionBinderCrossContext.GetInstance<BuildingsSignals>();
            _productionMenuScreenSignals = InjectionBinderCrossContext.GetInstance<ProductionMenuScreenSignals>();
            _inputSignals = InjectionBinderCrossContext.GetInstance<InputSignals>();

            IncomingSignals();
        }

        private void IncomingSignals()
        {
            _productionMenuScreenSignals.Outgoing.BuildTypeSelected.Connect(_buildingsSignals.Incoming.PlaceBuilding);

            _inputSignals.Outgoing.PointerPressed.Connect(_buildingsSignals.Incoming.PointerPressed);
            _inputSignals.Outgoing.PointerDragged.Connect(_buildingsSignals.Incoming.PointerDragged);
            _inputSignals.Outgoing.PointerReleased.Connect(_buildingsSignals.Incoming.PointerReleased);
        }

        public override void DestroyContext()
        {
            UnbindIncomingSignals();

            base.DestroyContext();
        }

        private void UnbindIncomingSignals()
        {
            _productionMenuScreenSignals.Outgoing.BuildTypeSelected.Disconnect();
            _inputSignals.Outgoing.PointerPressed.Disconnect();
            _inputSignals.Outgoing.PointerDragged.Disconnect();
            _inputSignals.Outgoing.PointerReleased.Disconnect();
        }
    }
}
