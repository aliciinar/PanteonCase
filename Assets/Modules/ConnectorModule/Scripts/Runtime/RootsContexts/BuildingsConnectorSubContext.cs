using FlowIoC.BaseModule.Connectors;
using FlowIoC.BaseModule.Contexts;
using Modules.BuildingsModule.BuildingInfoScreenModule.Signals;
using Modules.BuildingsModule.ProductionMenuScreenModule.Signals;
using Modules.BuildingsModule.Signals;
using Modules.GridModule.Signals;
using Modules.InputModule.Signals;

namespace Modules.ConnectorModule.RootsContexts
{
    /// <summary>
    /// What the buildings module is told: which building the player picked in its production menu,
    /// every press of the pointer that started on the world -
    /// where it began, where it was dragged, where it ended - and, from the grid, what a press landed on:
    /// a building selects it, a unit or nothing clears the selection. And what its info screen is told:
    /// the building selected on the board, or that nothing is; and what input is told: that a building
    /// is being placed, so a press's drags are wanted, and that the placement is over.
    /// </summary>
    public class BuildingsConnectorSubContext : Context
    {
        private BuildingsSignals _buildingsSignals;
        private ProductionMenuScreenSignals _productionMenuScreenSignals;
        private BuildingInfoScreenSignals _buildingInfoScreenSignals;
        private InputSignals _inputSignals;
        private GridSignals _gridSignals;

        public override void Setup()
        {
            base.Setup();

            _buildingsSignals = InjectionBinderCrossContext.GetInstance<BuildingsSignals>();
            _productionMenuScreenSignals = InjectionBinderCrossContext.GetInstance<ProductionMenuScreenSignals>();
            _buildingInfoScreenSignals = InjectionBinderCrossContext.GetInstance<BuildingInfoScreenSignals>();
            _inputSignals = InjectionBinderCrossContext.GetInstance<InputSignals>();
            _gridSignals = InjectionBinderCrossContext.GetInstance<GridSignals>();

            IncomingSignals();
            OutgoingSignals();
        }

        private void IncomingSignals()
        {
            _productionMenuScreenSignals.Outgoing.BuildTypeSelected.Connect(_buildingsSignals.Incoming.PlaceBuilding);

            _inputSignals.Outgoing.PointerPressed.Connect(_buildingsSignals.Incoming.PointerPressed);
            _inputSignals.Outgoing.PointerDragged.Connect(_buildingsSignals.Incoming.PointerDragged);
            _inputSignals.Outgoing.PointerReleased.Connect(_buildingsSignals.Incoming.PointerReleased);

            _gridSignals.Outgoing.BuildingPressed.Connect(_buildingsSignals.Incoming.SelectBuilding);
            // The unit itself is not the buildings' business - only that the press was not on a building.
            _gridSignals.Outgoing.UnitPressed.Connect(_ => _buildingsSignals.Incoming.ClearSelection.Dispatch());
            _gridSignals.Outgoing.EmptyPressed.Connect(_buildingsSignals.Incoming.ClearSelection);
        }

        private void OutgoingSignals()
        {
            // The info screen shows the selected building and closes when nothing is selected.
            _buildingsSignals.Outgoing.BuildingSelected.Connect(_buildingInfoScreenSignals.Incoming.ShowBuildingInfo);
            _buildingsSignals.Outgoing.SelectionCleared.Connect(_buildingInfoScreenSignals.Incoming.HideBuildingInfo);

            // Input reads a press's moves only while a building is being placed.
            _buildingsSignals.Outgoing.PlacementStarted.Connect(_inputSignals.Incoming.StartListeningToDrag);
            _buildingsSignals.Outgoing.PlacementEnded.Connect(_inputSignals.Incoming.StopListeningToDrag);
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
            _inputSignals.Outgoing.PointerPressed.Disconnect();
            _inputSignals.Outgoing.PointerDragged.Disconnect();
            _inputSignals.Outgoing.PointerReleased.Disconnect();
            _gridSignals.Outgoing.BuildingPressed.Disconnect();
            _gridSignals.Outgoing.UnitPressed.Disconnect();
            _gridSignals.Outgoing.EmptyPressed.Disconnect();
        }

        private void UnbindOutgoingSignals()
        {
            _buildingsSignals.Outgoing.BuildingSelected.Disconnect();
            _buildingsSignals.Outgoing.SelectionCleared.Disconnect();
            _buildingsSignals.Outgoing.PlacementStarted.Disconnect();
            _buildingsSignals.Outgoing.PlacementEnded.Disconnect();
        }
    }
}
