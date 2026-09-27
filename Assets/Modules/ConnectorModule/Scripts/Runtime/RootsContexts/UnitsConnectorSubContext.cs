using FlowIoC.BaseModule.Connectors;
using FlowIoC.BaseModule.Contexts;
using Modules.BuildingsModule.BuildingInfoScreenModule.Signals;
using Modules.GridModule.Signals;
using Modules.InputModule.Signals;
using Modules.UnitsModule.Signals;

namespace Modules.ConnectorModule.RootsContexts
{
    /// <summary>
    /// What the units module is told: a unit card clicked in the building info screen asks for a unit; and, from the
    /// grid, what a press landed on - a unit selects it, anything else clears the selection - and which free cell a
    /// secondary press ordered; and, from input, that a press landed on UI, which clears the selection too.
    /// </summary>
    public class UnitsConnectorSubContext : Context
    {
        private UnitsSignals _unitsSignals;
        private BuildingInfoScreenSignals _buildingInfoScreenSignals;
        private GridSignals _gridSignals;
        private InputSignals _inputSignals;

        public override void Setup()
        {
            base.Setup();

            _unitsSignals = InjectionBinderCrossContext.GetInstance<UnitsSignals>();
            _buildingInfoScreenSignals = InjectionBinderCrossContext.GetInstance<BuildingInfoScreenSignals>();
            _gridSignals = InjectionBinderCrossContext.GetInstance<GridSignals>();
            _inputSignals = InjectionBinderCrossContext.GetInstance<InputSignals>();

            IncomingSignals();
        }

        private void IncomingSignals()
        {
            _buildingInfoScreenSignals.Outgoing.UnitRequested.Connect(_unitsSignals.Incoming.SpawnUnit);

            _gridSignals.Outgoing.UnitPressed.Connect(_unitsSignals.Incoming.SelectUnit);
            _gridSignals.Outgoing.EmptyPressed.Connect(_unitsSignals.Incoming.ClearSelection);
            // The building itself is not the units' business - only that the press was not on a unit.
            _gridSignals.Outgoing.BuildingPressed.Connect(_ => _unitsSignals.Incoming.ClearSelection.Dispatch());

            _gridSignals.Outgoing.FreeCellSecondaryPressed.Connect(_unitsSignals.Incoming.MoveSelectedUnit);

            // A press on a HUD panel, a card or a button is not on a unit either.
            _inputSignals.Outgoing.PointerPressedOverUI.Connect(_unitsSignals.Incoming.ClearSelection);
        }

        public override void DestroyContext()
        {
            UnbindIncomingSignals();

            base.DestroyContext();
        }

        private void UnbindIncomingSignals()
        {
            _buildingInfoScreenSignals.Outgoing.UnitRequested.Disconnect();
            _gridSignals.Outgoing.UnitPressed.Disconnect();
            _gridSignals.Outgoing.EmptyPressed.Disconnect();
            _gridSignals.Outgoing.BuildingPressed.Disconnect();
            _gridSignals.Outgoing.FreeCellSecondaryPressed.Disconnect();
            _inputSignals.Outgoing.PointerPressedOverUI.Disconnect();
        }
    }
}
