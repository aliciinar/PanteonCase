using FlowIoC.BaseModule.Injectable.Attributes;
using FlowIoC.BaseModule.ViewsMediators.Mediator;
using FlowIoC.ScreenModule.Enums;
using FlowIoC.ScreenModule.ViewsMediators.Screen;
using Modules.BuildingsModule.BuildingInfoScreenModule.Signals;
using Modules.UnitsModule.Shared.Data.ValueObjects;

namespace Modules.BuildingsModule.BuildingInfoScreenModule.ViewsMediators
{
    /// <summary>
    /// Turns what the player does on the screen into signals: a unit card click, and the screen closing.
    /// Everything the screen shows is put there by the commands.
    /// </summary>
    public class BuildingInfoScreenMediator : IMediator
    {
        [Inject]       private BuildingInfoScreenView            _view            { get; set; }
        [InjectSignal] private BuildingInfoScreenSignals         _signals         { get; set; }
        [InjectSignal] private BuildingInfoScreenInternalSignals _internalSignals { get; set; }

        public void OnRegister()
        {
            _view.ShowCompleted += OnScreenShown;
            _view.HideCompleted += OnScreenHidden;
        }

        public void OnRemove()
        {
            _view.ShowCompleted -= OnScreenShown;
            _view.HideCompleted -= OnScreenHidden;
            _view.UnitClicked -= OnUnitClicked;
        }

        private void OnScreenShown(IScreenBody screen) => _view.UnitClicked += OnUnitClicked;

        private void OnScreenHidden(IScreenBody screen)
        {
            _view.UnitClicked -= OnUnitClicked;
            _internalSignals.InfoClosed.Dispatch(_view.RemoveUnits());
        }

        private void OnUnitClicked(UnitSpawnRequestVO request)
        {
            if (_view.Data.State != ScreenState.AvailableToSendSignal) return;

            _signals.Outgoing.UnitRequested.Dispatch(request);
        }
    }
}
