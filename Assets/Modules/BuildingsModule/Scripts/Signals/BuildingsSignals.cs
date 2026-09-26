using FlowIoC.BaseModule.Signals;

namespace Modules.BuildingsModule.Signals
{
    public class BuildingsSignals : ISignalHolder
    {
        public BuildingsSignalsIncoming Incoming = new();
        public BuildingsSignalsOutgoing Outgoing = new();
    }

    public class BuildingsSignalsIncoming
    {
    }

    public class BuildingsSignalsOutgoing
    {
    }
}
