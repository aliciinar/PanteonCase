using FlowIoC.BaseModule.Connectors;
using FlowIoC.BaseModule.Contexts;
using Modules.BuildingsModule.ProductionMenuScreenModule.Signals;
using Modules.GameBoardModule.Signals;

namespace Modules.ConnectorModule.RootsContexts
{
    /// <summary>What the board is told: which building the player picked in the production menu.</summary>
    public class GameBoardConnectorSubContext : Context
    {
        private GameBoardSignals _gameBoardSignals;
        private ProductionMenuScreenSignals _productionMenuScreenSignals;

        public override void Setup()
        {
            base.Setup();

            _gameBoardSignals = InjectionBinderCrossContext.GetInstance<GameBoardSignals>();
            _productionMenuScreenSignals = InjectionBinderCrossContext.GetInstance<ProductionMenuScreenSignals>();

            IncomingSignals();
        }

        private void IncomingSignals() =>
            _productionMenuScreenSignals.Outgoing.BuildTypeSelected.Connect(_gameBoardSignals.Incoming.PlaceBuilding);

        public override void DestroyContext()
        {
            UnbindIncomingSignals();

            base.DestroyContext();
        }

        private void UnbindIncomingSignals() =>
            _productionMenuScreenSignals.Outgoing.BuildTypeSelected.Disconnect();
    }
}
