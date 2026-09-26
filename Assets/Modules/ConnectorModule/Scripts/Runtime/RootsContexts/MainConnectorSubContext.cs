using FlowIoC.BaseModule.Connectors;
using FlowIoC.BaseModule.Contexts;
using Modules.BuildingsModule.ProductionMenuScreenModule.Signals;
using Modules.GameBoardModule.Signals;
using Modules.GameplayModule.InformationScreenModule.Signals;
using Modules.LoadingModule.LoadingScreenModule.Signals;
using Modules.MainModule.Signals;

namespace Modules.ConnectorModule.RootsContexts
{
    public class MainConnectorSubContext : Context
    {
        private MainSignals _mainSignals;
        private ProductionMenuScreenSignals _productionMenuScreenSignals;
        private InformationScreenSignals _informationScreenSignals;
        private GameBoardSignals _gameBoardSignals;
        private LoadingScreenSignals _loadingScreenSignals;

        public override void Setup()
        {
            base.Setup();

            _mainSignals = InjectionBinderCrossContext.GetInstance<MainSignals>();
            _productionMenuScreenSignals = InjectionBinderCrossContext.GetInstance<ProductionMenuScreenSignals>();
            _informationScreenSignals = InjectionBinderCrossContext.GetInstance<InformationScreenSignals>();
            _gameBoardSignals = InjectionBinderCrossContext.GetInstance<GameBoardSignals>();
            _loadingScreenSignals = InjectionBinderCrossContext.GetInstance<LoadingScreenSignals>();

            IncomingSignals();
            OutgoingSignals();
        }

        private void IncomingSignals()
        {
            // The loading screen's retry button asks Main to boot again; the set it names is Main's boot.
            _loadingScreenSignals.Outgoing.RetryClicked.Connect(_ => _mainSignals.Incoming.RetryBoot.Dispatch());
        }

        private void OutgoingSignals()
        {
            // Once the boot is done the player goes straight into the game: the two HUD panels open
            // and the board is built.
            _mainSignals.Outgoing.Started.Connect(_productionMenuScreenSignals.Incoming.OpenProductionMenuScreen);
            _mainSignals.Outgoing.Started.Connect(_informationScreenSignals.Incoming.OpenInformationScreen);
            _mainSignals.Outgoing.Started.Connect(_gameBoardSignals.Incoming.BuildBoard);

            // The HUD panels are laid out against the screen, so a new window size reaches both.
            _mainSignals.Outgoing.ScreenResized.Connect(_productionMenuScreenSignals.Incoming.ScreenResized);
            _mainSignals.Outgoing.ScreenResized.Connect(_informationScreenSignals.Incoming.ScreenResized);
        }

        public override void DestroyContext()
        {
            UnbindIncomingSignals();
            UnbindOutgoingSignals();

            base.DestroyContext();
        }

        private void UnbindIncomingSignals() =>
            _loadingScreenSignals.Outgoing.RetryClicked.Disconnect();

        private void UnbindOutgoingSignals()
        {
            _mainSignals.Outgoing.Started.Disconnect();
            _mainSignals.Outgoing.ScreenResized.Disconnect();
        }
    }
}
