using FlowIoC.BaseModule.Signals;
using UnityEngine;

namespace Modules.InputModule.Signals
{
    /// <summary>
    /// What the module says to its own commands. None of these leave the module, so they sit apart
    /// from the public holder in Shared rather than widening what the module offers.
    ///
    /// There is no Incoming and no Outgoing here. Those two halves say what a module accepts and
    /// what it announces across a boundary, and an internal signal never crosses one.
    ///
    /// The pointer as the view reads it, in screen pixels, before anything is decided about it.
    /// </summary>
    internal class InputInternalSignals : ISignalHolder
    {
        /// <summary>A press has started: the view is polled every frame from now until it ends.</summary>
        public Signal PressStarted = new();

        /// <summary>
        /// Once a frame while a press lasts, from IUpdateProvider: read the pointer. Kept out of the
        /// Flow Console, which would otherwise log a line every frame.
        /// </summary>
        public Signal PollPointer = new(hideCommandLog: true);

        public Signal<Vector2> PointerPressed = new();
        public Signal<Vector2> PointerDragged = new();
        public Signal<Vector2> PointerReleased = new();
    }
}
