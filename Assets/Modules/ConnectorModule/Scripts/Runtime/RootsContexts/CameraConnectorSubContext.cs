using FlowIoC.BaseModule.Connectors;
using FlowIoC.BaseModule.Contexts;
using Modules.BuildingsModule.ProductionMenuScreenModule.Signals;
using Modules.CameraModule.Signals;
using Modules.GameBoardModule.Signals;
using Modules.GameplayModule.InformationScreenModule.Signals;
using Modules.MainModule.Signals;

namespace Modules.ConnectorModule.RootsContexts
{
    /// <summary>
    /// What the camera is told: the board's bounds to keep in view, where the free screen area
    /// between the two HUD panels starts and ends, and when the window changes size.
    /// </summary>
    public class CameraConnectorSubContext : Context
    {
        private CameraSignals _cameraSignals;
        private GameBoardSignals _gameBoardSignals;
        private ProductionMenuScreenSignals _productionMenuScreenSignals;
        private InformationScreenSignals _informationScreenSignals;
        private MainSignals _mainSignals;

        public override void Setup()
        {
            base.Setup();

            _cameraSignals = InjectionBinderCrossContext.GetInstance<CameraSignals>();
            _gameBoardSignals = InjectionBinderCrossContext.GetInstance<GameBoardSignals>();
            _productionMenuScreenSignals = InjectionBinderCrossContext.GetInstance<ProductionMenuScreenSignals>();
            _informationScreenSignals = InjectionBinderCrossContext.GetInstance<InformationScreenSignals>();
            _mainSignals = InjectionBinderCrossContext.GetInstance<MainSignals>();

            IncomingSignals();
        }

        private void IncomingSignals()
        {
            _gameBoardSignals.Outgoing.BoardBuilt.Connect(_cameraSignals.Incoming.FitToBounds);
            _mainSignals.Outgoing.ScreenResized.Connect(_cameraSignals.Incoming.ScreenResized);

            // The production menu covers the left of the screen, so the free area starts at its right
            // edge; the information panel covers the right, so the free area ends at its left edge.
            _productionMenuScreenSignals.Outgoing.AreaChanged.Connect(_cameraSignals.Incoming.SetLeftInset, area => area.xMax);
            _informationScreenSignals.Outgoing.AreaChanged.Connect(_cameraSignals.Incoming.SetRightInset, area => area.xMin);
        }

        public override void DestroyContext()
        {
            UnbindIncomingSignals();

            base.DestroyContext();
        }

        private void UnbindIncomingSignals()
        {
            _gameBoardSignals.Outgoing.BoardBuilt.Disconnect();
            _mainSignals.Outgoing.ScreenResized.Disconnect();
            _productionMenuScreenSignals.Outgoing.AreaChanged.Disconnect();
            _informationScreenSignals.Outgoing.AreaChanged.Disconnect();
        }
    }
}
