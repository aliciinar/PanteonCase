using System.Collections.Generic;
using FlowIoC.BaseModule.Signals;
using Modules.BuildingsModule.ProductionMenuScreenModule.Entities;

namespace Modules.BuildingsModule.ProductionMenuScreenModule.Signals
{
    /// <summary>
    /// What the module says to its own commands. None of these leave the module.
    ///
    /// There is no Incoming and no Outgoing here. Those two halves say what a module accepts and
    /// what it announces across a boundary, and an internal signal never crosses one.
    /// </summary>
    internal class ProductionMenuScreenInternalSignals : ISignalHolder
    {
        /// <summary>The menu scrolled or was laid out again: work out the visible rows.</summary>
        public Signal Scrolled = new(hideCommandLog: true);

        /// <summary>These rows came into view and need cards.</summary>
        public Signal<List<int>> RowsEntered = new(hideCommandLog: true);

        /// <summary>These rows left the view; their cards go back to the pool.</summary>
        public Signal<List<int>> RowsLeft = new(hideCommandLog: true);

        /// <summary>The menu closed. Carries every card it held, to go back to the pool.</summary>
        public Signal<List<ProductionItem>> MenuClosed = new();
    }
}
