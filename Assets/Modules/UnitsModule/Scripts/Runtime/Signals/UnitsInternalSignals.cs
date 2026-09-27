using FlowIoC.BaseModule.Signals;
using Modules.GridModule.Shared.Data.ValueObjects;
using Modules.UnitsModule.Data.ValueObjects;
using UnityEngine;

namespace Modules.UnitsModule.Signals
{
    /// <summary>
    /// What the module says to its own commands. None of these leave the module, so they sit apart
    /// from the public holder in Shared rather than widening what the module offers.
    ///
    /// There is no Incoming and no Outgoing here. Those two halves say what a module accepts and
    /// what it announces across a boundary, and an internal signal never crosses one.
    /// </summary>
    internal class UnitsInternalSignals : ISignalHolder
    {
        /// <summary>A unit the units view puts on the board and walks along its path.</summary>
        public Signal<PlacedUnitVO> ShowUnit = new();

        /// <summary>A unit on the board walks along new world points.</summary>
        public Signal<UnitWalkVO> MoveUnit = new();

        /// <summary>This unit is the selected one: tint it with this colour.</summary>
        public Signal<BoardUnitVO, Color> ShowUnitSelected = new();

        /// <summary>This unit is not selected any more: take its tint off.</summary>
        public Signal<BoardUnitVO> HideUnitSelected = new();

        /// <summary>
        /// A walking unit starts stepping into the cell whose centre is this world point. Kept out of the Flow Console -
        /// it comes once per cell of every walk.
        /// </summary>
        public Signal<BoardUnitVO, Vector3> UnitStepped = new(hideCommandLog: true);
    }
}
