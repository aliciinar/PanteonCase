using FlowIoC.BaseModule.Connectors;
using FlowIoC.BaseModule.Contexts;
using Modules.GameBoardModule.Signals;
using Modules.GameplayModule.GameplayScreenModule.Signals;
using Modules.LoadingModule.LoadingScreenModule.Signals;
using Modules.MainModule.Signals;

namespace Modules.ConnectorModule.RootsContexts
{
    public class MainConnectorSubContext : Context
    {
        private MainSignals _mainSignals;
        private GameplayScreenSignals _gameplayScreenSignals;
        private GameBoardSignals _gameBoardSignals;
        private LoadingScreenSignals _loadingScreenSignals;

        public override void Setup()
        {
            base.Setup();

            _mainSignals = InjectionBinderCrossContext.GetInstance<MainSignals>();
            _gameplayScreenSignals = InjectionBinderCrossContext.GetInstance<GameplayScreenSignals>();
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
            // Once the boot is done the player goes straight into the game: the HUD opens and the board is built.
            _mainSignals.Outgoing.Started.Connect(_gameplayScreenSignals.Incoming.OpenGameplayScreen);
            _mainSignals.Outgoing.Started.Connect(_gameBoardSignals.Incoming.BuildBoard);
        }

        public override void DestroyContext()
        {
            UnbindIncomingSignals();
            UnbindOutgoingSignals();

            base.DestroyContext();
        }

        private void UnbindIncomingSignals() =>
            _loadingScreenSignals.Outgoing.RetryClicked.Disconnect();

        private void UnbindOutgoingSignals() =>
            _mainSignals.Outgoing.Started.Disconnect();
    }
}
