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
    /// them (ListenToDrag - a building being placed); otherwise only where it starts and ends. The
    /// secondary button (Board/SecondaryPress - the right mouse button) is reported where it is pressed.
    ///
    /// The view has no Update of its own. It only says when a button goes down; from then until both are
    /// up (PressesEnded) the module's commands keep a PollPointer signal on FlowIoC's IUpdateProvider and
    /// the Mediator calls Poll with it every frame, so a frame with the buttons up costs nothing. Polled
    /// rather than read from action callbacks: whoever hears these asks the EventSystem whether the
    /// pointer is over UI, and that answer is wrong inside input callbacks.
    /// </summary>
    [RequireComponent(typeof(ViewInjector))]
    public class InputView : MonoBehaviour, IView
    {
        public bool IsRegistered { get; set; }

        [Tooltip("Board/Point - the pointer's screen position.")]
        [SerializeField] private InputActionReference _point;

        [Tooltip("Board/Press - the pointer's main button, or a touch.")]
        [SerializeField] private InputActionReference _press;

        [Tooltip("Board/SecondaryPress - the pointer's secondary button (right mouse button).")]
        [SerializeField] private InputActionReference _secondaryPress;

        /// <summary>A button went down - Poll from this frame until PressesEnded.</summary>
        public event Action PressStarted;

        public event Action<Vector2> PointerPressed;
        public event Action<Vector2> PointerDragged;
        public event Action<Vector2> PointerReleased;
        public event Action<Vector2> SecondaryPressed;

        /// <summary>Neither button is down any more - Poll is not needed until the next PressStarted.</summary>
        public event Action PressesEnded;

        private Vector2 _lastPosition;
        private bool _listensToDrag;

        private void OnEnable()
        {
            _point.action.Enable();
            _press.action.Enable();
            _secondaryPress.action.Enable();
            _press.action.started += OnPressStarted;
            _secondaryPress.action.started += OnPressStarted;
        }

        private void OnDisable()
        {
            _press.action.started -= OnPressStarted;
            _secondaryPress.action.started -= OnPressStarted;
        }

        // Input is processed before Update, so a Poll registered here still runs this frame and sees
        // the press.
        private void OnPressStarted(InputAction.CallbackContext context) => PressStarted?.Invoke();

        /// <summary>Whether a press's moves are read. Off until someone needs them.</summary>
        public void ListenToDrag(bool listen) => _listensToDrag = listen;

        /// <summary>
        /// Reads the buttons for this frame: where a press started, where it was dragged (only while listened
        /// to), where it ended, where the secondary button went down - and whether both are up now.
        /// </summary>
        public void Poll()
        {
            Vector2 position = _point.action.ReadValue<Vector2>();
            InputAction press = _press.action;
            InputAction secondaryPress = _secondaryPress.action;

            if (press.WasPressedThisFrame())
                PointerPressed?.Invoke(position);
            else if (_listensToDrag && press.IsPressed() && position != _lastPosition)
                PointerDragged?.Invoke(position);

            if (press.WasReleasedThisFrame())
                PointerReleased?.Invoke(position);

            if (secondaryPress.WasPressedThisFrame())
                SecondaryPressed?.Invoke(position);

            _lastPosition = position;

            if (!press.IsPressed() && !secondaryPress.IsPressed())
                PressesEnded?.Invoke();
        }
    }
}
