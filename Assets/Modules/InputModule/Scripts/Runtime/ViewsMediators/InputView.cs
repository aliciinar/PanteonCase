using System;
using FlowIoC.BaseModule.Injectable.Components;
using FlowIoC.BaseModule.ViewsMediators.View;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Modules.InputModule.ViewsMediators
{
    /// <summary>
    /// Reads the pointer - mouse, touch or pen - through the project-wide Board action map and reports
    /// a press from start to end, in screen pixels. A press's moves are read only while someone needs
    /// them (ListenToDrag - a building being placed); otherwise only where it starts and ends.
    ///
    /// The view has no Update of its own. It only says when a press starts; from then until the release
    /// the module's commands keep a PollPointer signal on FlowIoC's IUpdateProvider and the Mediator
    /// calls Poll with it every frame, so a frame with the pointer up costs nothing. Polled rather than
    /// read from action callbacks: whoever hears these asks the EventSystem whether the pointer is over
    /// UI, and that answer is wrong inside input callbacks.
    /// </summary>
    [RequireComponent(typeof(ViewInjector))]
    public class InputView : MonoBehaviour, IView
    {
        public bool IsRegistered { get; set; }

        [Tooltip("Board/Point - the pointer's screen position.")]
        [SerializeField] private InputActionReference _point;

        [Tooltip("Board/Press - the pointer's main button, or a touch.")]
        [SerializeField] private InputActionReference _press;

        /// <summary>A press has started - Poll from this frame until PointerReleased.</summary>
        public event Action PressStarted;

        public event Action<Vector2> PointerPressed;
        public event Action<Vector2> PointerDragged;
        public event Action<Vector2> PointerReleased;

        private Vector2 _lastPosition;
        private bool _listensToDrag;

        private void OnEnable()
        {
            _point.action.Enable();
            _press.action.Enable();
            _press.action.started += OnPressStarted;
        }

        private void OnDisable() => _press.action.started -= OnPressStarted;

        // Input is processed before Update, so a Poll registered here still runs this frame and sees
        // the press.
        private void OnPressStarted(InputAction.CallbackContext context) => PressStarted?.Invoke();

        /// <summary>Whether a press's moves are read. Off until someone needs them.</summary>
        public void ListenToDrag(bool listen) => _listensToDrag = listen;

        /// <summary>Reads the press for this frame: where it started, where it was dragged (only while listened to), where it ended.</summary>
        public void Poll()
        {
            Vector2 position = _point.action.ReadValue<Vector2>();
            InputAction press = _press.action;

            if (press.WasPressedThisFrame())
                PointerPressed?.Invoke(position);
            else if (_listensToDrag && press.IsPressed() && position != _lastPosition)
                PointerDragged?.Invoke(position);

            if (press.WasReleasedThisFrame())
                PointerReleased?.Invoke(position);

            _lastPosition = position;
        }
    }
}
