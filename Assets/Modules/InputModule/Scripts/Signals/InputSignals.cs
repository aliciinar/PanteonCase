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
    }

    /// <summary>
    /// A press of the pointer that started on the game world, not over UI, from start to end. Every
    /// position is in world units.
    /// </summary>
    public class InputSignalsOutgoing
    {
        /// <summary>The pointer was pressed on the world here.</summary>
        public Signal<Vector2> PointerPressed = new();

        /// <summary>The press goes on and the pointer has moved here. Kept out of the Flow Console - it fires every frame of a drag.</summary>
        public Signal<Vector2> PointerDragged = new(hideCommandLog: true);

        /// <summary>The press ended here.</summary>
        public Signal<Vector2> PointerReleased = new();
    }
}
