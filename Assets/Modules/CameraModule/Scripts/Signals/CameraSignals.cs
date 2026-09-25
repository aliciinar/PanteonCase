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
    }

    public class CameraSignalsOutgoing
    {
    }
}
