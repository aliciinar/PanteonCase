using FlowIoC.BaseModule.Connectors;
using FlowIoC.BaseModule.Contexts;
using Modules.CameraModule.Signals;
using Modules.GameBoardModule.Signals;
using Modules.GameplayModule.GameplayScreenModule.Signals;
using Modules.MainModule.Signals;

namespace Modules.ConnectorModule.RootsContexts
{
    /// <summary>
    /// What the camera is told: the board's bounds to keep in view, the free screen area between the
    /// gameplay HUD's panels to draw into, and when the window changes size.
    /// </summary>
    public class CameraConnectorSubContext : Context
    {
        private CameraSignals _cameraSignals;
        private GameBoardSignals _gameBoardSignals;
        private GameplayScreenSignals _gameplayScreenSignals;
        private MainSignals _mainSignals;

        public override void Setup()
        {
            base.Setup();

            _cameraSignals = InjectionBinderCrossContext.GetInstance<CameraSignals>();
            _gameBoardSignals = InjectionBinderCrossContext.GetInstance<GameBoardSignals>();
            _gameplayScreenSignals = InjectionBinderCrossContext.GetInstance<GameplayScreenSignals>();
            _mainSignals = InjectionBinderCrossContext.GetInstance<MainSignals>();

            IncomingSignals();
        }

        private void IncomingSignals()
        {
            _gameBoardSignals.Outgoing.BoardBuilt.Connect(_cameraSignals.Incoming.FitToBounds);
            _gameplayScreenSignals.Outgoing.PlayAreaChanged.Connect(_cameraSignals.Incoming.SetViewport);
            _mainSignals.Outgoing.ScreenResized.Connect(_cameraSignals.Incoming.ScreenResized);
        }

        public override void DestroyContext()
        {
            UnbindIncomingSignals();

            base.DestroyContext();
        }

        private void UnbindIncomingSignals()
        {
            _gameBoardSignals.Outgoing.BoardBuilt.Disconnect();
            _gameplayScreenSignals.Outgoing.PlayAreaChanged.Disconnect();
            _mainSignals.Outgoing.ScreenResized.Disconnect();
        }
    }
}
