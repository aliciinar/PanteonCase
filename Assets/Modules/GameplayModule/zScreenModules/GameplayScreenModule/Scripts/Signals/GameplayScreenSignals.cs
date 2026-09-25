using FlowIoC.BaseModule.Signals;
using UnityEngine;

namespace Modules.GameplayModule.GameplayScreenModule.Signals
{
    public class GameplayScreenSignals : ISignalHolder
    {
        public GameplayScreenSignalsIncoming Incoming = new();
        public GameplayScreenSignalsOutgoing Outgoing = new();
    }

    public class GameplayScreenSignalsIncoming
    {
        public Signal OpenGameplayScreen = new();
    }

    public class GameplayScreenSignalsOutgoing
    {
        /// <summary>
        /// The free area between the screen's side panels, normalised to the screen (0-1, origin
        /// bottom-left). Sent when the screen shows and again whenever the window is resized.
        /// </summary>
        public Signal<Rect> PlayAreaChanged = new();
    }
}
