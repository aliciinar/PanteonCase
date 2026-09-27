using FlowIoC.BaseModule.Signals;

namespace Modules.GameplayModule.Signals
{
    public class GameplaySignals : ISignalHolder
    {
        public GameplaySignalsIncoming Incoming = new();
        public GameplaySignalsOutgoing Outgoing = new();
    }

    public class GameplaySignalsIncoming
    {
        /// <summary>An action started: the game takes no order until Unlock.</summary>
        public Signal Lock = new();

        /// <summary>The action ended: orders are taken again.</summary>
        public Signal Unlock = new();
    }

    public class GameplaySignalsOutgoing
    {
    }
}
