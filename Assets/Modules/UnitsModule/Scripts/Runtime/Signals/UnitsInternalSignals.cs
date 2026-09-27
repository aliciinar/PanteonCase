using FlowIoC.BaseModule.Signals;
using Modules.UnitsModule.Data.ValueObjects;

namespace Modules.UnitsModule.Signals
{
    /// <summary>
    /// What the module says to its own commands. None of these leave the module, so they sit apart
    /// from the public holder in Shared rather than widening what the module offers.
    ///
    /// There is no Incoming and no Outgoing here. Those two halves say what a module accepts and
    /// what it announces across a boundary, and an internal signal never crosses one.
    ///
    /// Both are dispatched by the tweens the board-unit commands build on a unit object, when the moment comes.
    /// </summary>
    internal class UnitsInternalSignals : ISignalHolder
    {
        /// <summary>The attacker's lunge reached its target: the strike lands now.</summary>
        public Signal<UnitStrikeVO> UnitStruck = new();

        /// <summary>A unit's action - its walk, or its walk and strike - is over.</summary>
        public Signal UnitActionFinished = new();
    }
}
