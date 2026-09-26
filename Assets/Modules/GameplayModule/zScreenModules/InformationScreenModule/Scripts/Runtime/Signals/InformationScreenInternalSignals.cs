using FlowIoC.BaseModule.Signals;

namespace Modules.GameplayModule.InformationScreenModule.Signals
{
    /// <summary>
    /// What the module says to its own commands. None of these leave the module.
    ///
    /// There is no Incoming and no Outgoing here. Those two halves say what a module accepts and
    /// what it announces across a boundary, and an internal signal never crosses one.
    /// </summary>
    internal class InformationScreenInternalSignals : ISignalHolder
    {
    }
}
