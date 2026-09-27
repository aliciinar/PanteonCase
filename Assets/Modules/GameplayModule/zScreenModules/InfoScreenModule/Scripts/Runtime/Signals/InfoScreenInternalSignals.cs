using System.Collections.Generic;
using FlowIoC.BaseModule.Signals;
using Modules.GameplayModule.InfoScreenModule.Entities;

namespace Modules.GameplayModule.InfoScreenModule.Signals
{
    /// <summary>
    /// What the module says to its own commands. None of these leave the module, so they sit apart
    /// from the public holder in Shared rather than widening what the module offers.
    ///
    /// There is no Incoming and no Outgoing here. Those two halves say what a module accepts and
    /// what it announces across a boundary, and an internal signal never crosses one.
    /// </summary>
    internal class InfoScreenInternalSignals : ISignalHolder
    {
        /// <summary>The screen closed. Carries every unit card it held, to go back to the pool.</summary>
        public Signal<List<UnitItem>> InfoClosed = new();
    }
}
