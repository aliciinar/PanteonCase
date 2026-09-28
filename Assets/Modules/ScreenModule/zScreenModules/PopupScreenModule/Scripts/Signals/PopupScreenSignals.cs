using FlowIoC.BaseModule.Signals;

namespace Modules.ScreenModule.PopupScreenModule.Signals
{
    public class PopupScreenSignals : ISignalHolder
    {
        public PopupScreenSignalsIncoming Incoming = new();
        public PopupScreenSignalsOutgoing Outgoing = new();
    }

    public class PopupScreenSignalsIncoming
    {
        /// <summary>
        /// Show this message in the popup, over every other screen. The popup opens when it is closed; when it is
        /// already open the new message takes the old one's place.
        /// </summary>
        public Signal<string> ShowPopup = new();
    }

    public class PopupScreenSignalsOutgoing
    {
    }
}
