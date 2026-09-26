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
        /// Where on screen the free area starts from the left, normalised (0-1): the right edge of
        /// whatever UI covers the left of the screen. 0 until told otherwise.
        /// </summary>
        public Signal<float> SetLeftInset = new();

        /// <summary>
        /// Where on screen the free area ends on the right, normalised (0-1): the left edge of
        /// whatever UI covers the right of the screen. 1 until told otherwise.
        /// </summary>
        public Signal<float> SetRightInset = new();

        /// <summary>A world rect the camera has to keep entirely in view, centred.</summary>
        public Signal<Rect> FitToBounds = new();

        /// <summary>The window changed size (new size in pixels), so the viewport's aspect did too: fit again.</summary>
        public Signal<Vector2Int> ScreenResized = new();
    }

    public class CameraSignalsOutgoing
    {
    }
}
