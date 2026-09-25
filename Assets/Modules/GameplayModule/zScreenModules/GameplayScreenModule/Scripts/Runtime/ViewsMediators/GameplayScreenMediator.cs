using FlowIoC.BaseModule.Injectable.Attributes;
using FlowIoC.BaseModule.ViewsMediators.Mediator;
using FlowIoC.ScreenModule.ViewsMediators.Screen;
using Modules.GameplayModule.GameplayScreenModule.Signals;
using UnityEngine;

namespace Modules.GameplayModule.GameplayScreenModule.ViewsMediators
{
    public class GameplayScreenMediator : IMediator
    {
        [Inject]       private GameplayScreenView    _view    { get; set; }
        [InjectSignal] private GameplayScreenSignals _signals { get; set; }

        public virtual void OnRegister()
        {
            _view.ShowCompleted += OnScreenShown;
            _view.HideCompleted += OnScreenHidden;
        }

        public virtual void OnRemove()
        {
            _view.ShowCompleted -= OnScreenShown;
            _view.HideCompleted -= OnScreenHidden;
            _view.PlayAreaChanged -= OnPlayAreaChanged;
        }

        private void OnScreenShown(IScreenBody screen)
        {
            _view.PlayAreaChanged += OnPlayAreaChanged;
            _view.ReportPlayArea();
        }

        private void OnScreenHidden(IScreenBody screen)
        {
            _view.PlayAreaChanged -= OnPlayAreaChanged;
        }

        // A layout measurement rather than a tap, so it is not held back while the screen animates.
        private void OnPlayAreaChanged(Rect playArea) => _signals.Outgoing.PlayAreaChanged.Dispatch(playArea);
    }
}
