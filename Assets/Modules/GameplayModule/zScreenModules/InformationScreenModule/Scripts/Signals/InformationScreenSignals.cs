using FlowIoC.BaseModule.Signals;
using UnityEngine;

namespace Modules.GameplayModule.InformationScreenModule.Signals
{
    public class InformationScreenSignals : ISignalHolder
    {
        public InformationScreenSignalsIncoming Incoming = new();
        public InformationScreenSignalsOutgoing Outgoing = new();
    }

    public class InformationScreenSignalsIncoming
    {
        public Signal OpenInformationScreen = new();

        /// <summary>The window changed size (new size in pixels): measure the panel again.</summary>
        public Signal<Vector2Int> ScreenResized = new();
    }

    public class InformationScreenSignalsOutgoing
    {
        /// <summary>
        /// The screen rect the information panel covers, normalised (0-1, origin bottom-left). Sent
        /// when the screen shows and whenever the window is resized.
        /// </summary>
        public Signal<Rect> AreaChanged = new();
    }
}
