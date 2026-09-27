using FlowIoC.BaseModule.Signals;
using UnityEngine;

namespace Modules.InputModule.Signals
{
    public class InputSignals : ISignalHolder
    {
        public InputSignalsIncoming Incoming = new();
        public InputSignalsOutgoing Outgoing = new();
    }

    public class InputSignalsIncoming
    {
        /// <summary>
        /// Someone needs where a press is dragged - a building being placed: read the pointer's moves
        /// while it is pressed, from now until StopListeningToDrag. Presses and releases are always heard.
        /// </summary>
        public Signal StartListeningToDrag = new();

        /// <summary>Nobody needs a press's drags any more - the placement was confirmed or cancelled.</summary>
        public Signal StopListeningToDrag = new();
    }

    /// <summary>
    /// A press of the pointer that started on the game world, not over UI, from start to end. Every
    /// position is in world units.
    /// </summary>
    public class InputSignalsOutgoing
    {
        /// <summary>The pointer was pressed on the world here.</summary>
        public Signal<Vector2> PointerPressed = new();

        /// <summary>
        /// The press goes on and the pointer has moved here - only between StartListeningToDrag and
        /// StopListeningToDrag. Kept out of the Flow Console - it fires every frame of a drag.
        /// </summary>
        public Signal<Vector2> PointerDragged = new(hideCommandLog: true);

        /// <summary>The press ended here.</summary>
        public Signal<Vector2> PointerReleased = new();
    }
}
