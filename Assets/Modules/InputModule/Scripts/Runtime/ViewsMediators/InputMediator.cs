using FlowIoC.BaseModule.Injectable.Attributes;
using FlowIoC.BaseModule.ViewsMediators.Mediator;
using Modules.InputModule.Signals;
using UnityEngine;

namespace Modules.InputModule.ViewsMediators
{
    /// <summary>
    /// Hands what the pointer does to the module's commands, which decide whether it is announced, has
    /// the view read the pointer whenever the commands' polling asks for it (PollPointer), and tells it
    /// whether a press's moves are wanted (DragListening).
    /// </summary>
    public class InputMediator : IMediator
    {
        [Inject]       private InputView            _view            { get; set; }
        [InjectSignal] private InputInternalSignals _internalSignals { get; set; }

        public void OnRegister()
        {
            _internalSignals.PollPointer.AddListener(_view.Poll);
            _internalSignals.DragListening.AddListener(_view.ListenToDrag);

            _view.PressStarted += OnPressStarted;
            _view.PressesEnded += OnPressesEnded;
            _view.PointerPressed += OnPointerPressed;
            _view.PointerDragged += OnPointerDragged;
            _view.PointerReleased += OnPointerReleased;
            _view.SecondaryPressed += OnSecondaryPressed;
        }

        public void OnRemove()
        {
            _internalSignals.PollPointer.RemoveListener(_view.Poll);
            _internalSignals.DragListening.RemoveListener(_view.ListenToDrag);

            _view.PressStarted -= OnPressStarted;
            _view.PressesEnded -= OnPressesEnded;
            _view.PointerPressed -= OnPointerPressed;
            _view.PointerDragged -= OnPointerDragged;
            _view.PointerReleased -= OnPointerReleased;
            _view.SecondaryPressed -= OnSecondaryPressed;
        }

        private void OnPressStarted() => _internalSignals.PressStarted.Dispatch();

        private void OnPressesEnded() => _internalSignals.PressesEnded.Dispatch();

        private void OnPointerPressed(Vector2 screenPosition) => _internalSignals.PointerPressed.Dispatch(screenPosition);

        private void OnPointerDragged(Vector2 screenPosition) => _internalSignals.PointerDragged.Dispatch(screenPosition);

        private void OnPointerReleased(Vector2 screenPosition) => _internalSignals.PointerReleased.Dispatch(screenPosition);

        private void OnSecondaryPressed(Vector2 screenPosition) => _internalSignals.SecondaryPressed.Dispatch(screenPosition);
    }
}
