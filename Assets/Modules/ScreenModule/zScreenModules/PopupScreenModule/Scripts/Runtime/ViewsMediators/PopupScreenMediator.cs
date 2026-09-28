using FlowIoC.BaseModule.Injectable.Attributes;
using FlowIoC.BaseModule.ViewsMediators.Mediator;
using FlowIoC.ScreenModule.Enums;
using FlowIoC.ScreenModule.ViewsMediators.Screen;

namespace Modules.ScreenModule.PopupScreenModule.ViewsMediators
{
    /// <summary>
    /// Closes the popup on a click on the panel behind it - a dismissal that decides nothing, so it is the Mediator's
    /// own. The message is put on it by the command that opened it.
    /// </summary>
    public class PopupScreenMediator : IMediator
    {
        [Inject] private PopupScreenView _view { get; set; }

        public void OnRegister()
        {
            _view.ShowCompleted += OnScreenShown;
            _view.HideCompleted += OnScreenHidden;
        }

        public void OnRemove()
        {
            _view.ShowCompleted -= OnScreenShown;
            _view.HideCompleted -= OnScreenHidden;
            _view.Dismissed -= OnDismissed;
        }

        private void OnScreenShown(IScreenBody screen) => _view.Dismissed += OnDismissed;

        private void OnScreenHidden(IScreenBody screen) => _view.Dismissed -= OnDismissed;

        private void OnDismissed()
        {
            if (_view.Data.State != ScreenState.AvailableToSendSignal) return;

            _view.Hide();
        }
    }
}
