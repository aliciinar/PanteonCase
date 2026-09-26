using FlowIoC.BaseModule.Connectors;
using FlowIoC.BaseModule.Contexts;
using Modules.BuildingsModule.Signals;
using Modules.UnitsModule.Signals;

namespace Modules.ConnectorModule.RootsContexts
{
    /// <summary>What the units module is told: a building asks for a unit.</summary>
    public class UnitsConnectorSubContext : Context
    {
        private UnitsSignals _unitsSignals;
        private BuildingsSignals _buildingsSignals;

        public override void Setup()
        {
            base.Setup();

            _unitsSignals = InjectionBinderCrossContext.GetInstance<UnitsSignals>();
            _buildingsSignals = InjectionBinderCrossContext.GetInstance<BuildingsSignals>();

            IncomingSignals();
        }

        private void IncomingSignals() =>
            _buildingsSignals.Outgoing.UnitRequested.Connect(_unitsSignals.Incoming.SpawnUnit);

        public override void DestroyContext()
        {
            UnbindIncomingSignals();

            base.DestroyContext();
        }

        private void UnbindIncomingSignals() => _buildingsSignals.Outgoing.UnitRequested.Disconnect();
    }
}
