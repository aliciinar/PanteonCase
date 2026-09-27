using FlowIoC.BaseModule.Connectors;
using FlowIoC.BaseModule.Contexts;
using Modules.BuildingsModule.BuildingInfoScreenModule.Signals;
using Modules.UnitsModule.Signals;

namespace Modules.ConnectorModule.RootsContexts
{
    /// <summary>What the units module is told: a unit card clicked in the building info screen asks for a unit.</summary>
    public class UnitsConnectorSubContext : Context
    {
        private UnitsSignals _unitsSignals;
        private BuildingInfoScreenSignals _buildingInfoScreenSignals;

        public override void Setup()
        {
            base.Setup();

            _unitsSignals = InjectionBinderCrossContext.GetInstance<UnitsSignals>();
            _buildingInfoScreenSignals = InjectionBinderCrossContext.GetInstance<BuildingInfoScreenSignals>();

            IncomingSignals();
        }

        private void IncomingSignals() =>
            _buildingInfoScreenSignals.Outgoing.UnitRequested.Connect(_unitsSignals.Incoming.SpawnUnit);

        public override void DestroyContext()
        {
            UnbindIncomingSignals();

            base.DestroyContext();
        }

        private void UnbindIncomingSignals() => _buildingInfoScreenSignals.Outgoing.UnitRequested.Disconnect();
    }
}
