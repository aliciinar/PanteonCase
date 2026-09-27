using FlowIoC.BaseModule.Connectors;
using FlowIoC.BaseModule.Contexts;
using Modules.BuildingsModule.BuildingInfoScreenModule.Signals;
using Modules.BuildingsModule.ProductionMenuScreenModule.Signals;
using Modules.BuildingsModule.Signals;
using Modules.InputModule.Signals;

namespace Modules.ConnectorModule.RootsContexts
{
    /// <summary>
    /// What the buildings module is told: which building the player picked in its production menu,
    /// which unit they picked in its info screen, and every press of the pointer that started on the
    /// world - where it began, where it was dragged, where it ended. And what its info screen is told:
    /// the building selected on the board, or that nothing is.
    /// </summary>
    public class BuildingsConnectorSubContext : Context
    {
        private BuildingsSignals _buildingsSignals;
        private ProductionMenuScreenSignals _productionMenuScreenSignals;
        private BuildingInfoScreenSignals _buildingInfoScreenSignals;
        private InputSignals _inputSignals;

        public override void Setup()
        {
            base.Setup();

            _buildingsSignals = InjectionBinderCrossContext.GetInstance<BuildingsSignals>();
            _productionMenuScreenSignals = InjectionBinderCrossContext.GetInstance<ProductionMenuScreenSignals>();
            _buildingInfoScreenSignals = InjectionBinderCrossContext.GetInstance<BuildingInfoScreenSignals>();
            _inputSignals = InjectionBinderCrossContext.GetInstance<InputSignals>();

            IncomingSignals();
            OutgoingSignals();
        }

        private void IncomingSignals()
        {
            _productionMenuScreenSignals.Outgoing.BuildTypeSelected.Connect(_buildingsSignals.Incoming.PlaceBuilding);
            _buildingInfoScreenSignals.Outgoing.UnitClicked.Connect(_buildingsSignals.Incoming.ProduceUnit);

            _inputSignals.Outgoing.PointerPressed.Connect(_buildingsSignals.Incoming.PointerPressed);
            _inputSignals.Outgoing.PointerDragged.Connect(_buildingsSignals.Incoming.PointerDragged);
            _inputSignals.Outgoing.PointerReleased.Connect(_buildingsSignals.Incoming.PointerReleased);
        }

        private void OutgoingSignals()
        {
            // The info screen shows the selected building and closes when nothing is selected.
            _buildingsSignals.Outgoing.BuildingSelected.Connect(_buildingInfoScreenSignals.Incoming.ShowBuildingInfo);
            _buildingsSignals.Outgoing.SelectionCleared.Connect(_buildingInfoScreenSignals.Incoming.HideBuildingInfo);
        }

        public override void DestroyContext()
        {
            UnbindIncomingSignals();
            UnbindOutgoingSignals();

            base.DestroyContext();
        }

        private void UnbindIncomingSignals()
        {
            _productionMenuScreenSignals.Outgoing.BuildTypeSelected.Disconnect();
            _buildingInfoScreenSignals.Outgoing.UnitClicked.Disconnect();
            _inputSignals.Outgoing.PointerPressed.Disconnect();
            _inputSignals.Outgoing.PointerDragged.Disconnect();
            _inputSignals.Outgoing.PointerReleased.Disconnect();
        }

        private void UnbindOutgoingSignals()
        {
            _buildingsSignals.Outgoing.BuildingSelected.Disconnect();
            _buildingsSignals.Outgoing.SelectionCleared.Disconnect();
        }
    }
}
