using FlowIoC.BaseModule.Signals;
using UnityEngine;

namespace Modules.CameraModule.Signals
{
    public class CameraSignals : ISignalHolder
    {
        public CameraSignalsIncoming Incoming = new();
        public CameraSignalsOutgoing Outgoing = new();
    }

    public class CameraSignalsIncoming
    {
        /// <summary>
        /// The part of the screen the camera may draw into, normalised (0-1, origin bottom-left) - the
        /// free area between whatever UI surrounds the game. Until it arrives the camera uses the whole screen.
        /// </summary>
        public Signal<Rect> SetViewport = new();

        /// <summary>A world rect the camera has to keep entirely in view, centred.</summary>
        public Signal<Rect> FitToBounds = new();

        /// <summary>The window changed size (new size in pixels), so the viewport's aspect did too: fit again.</summary>
        public Signal<Vector2Int> ScreenResized = new();
    }

    public class CameraSignalsOutgoing
    {
    }
}
