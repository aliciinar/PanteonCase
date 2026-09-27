using FlowIoC.BaseModule.Signals;
using Modules.GridModule.Shared.Data.ValueObjects;
using UnityEngine;

namespace Modules.GridModule.Signals
{
    public class GridSignals : ISignalHolder
    {
        public GridSignalsIncoming Incoming = new();
        public GridSignalsOutgoing Outgoing = new();
    }

    public class GridSignalsIncoming
    {
        /// <summary>A press began on the world here (world units): say what it landed on.</summary>
        public Signal<Vector2> PointerPressed = new();

        /// <summary>The secondary button was pressed on the world here (world units): say what it ordered.</summary>
        public Signal<Vector2> PointerSecondaryPressed = new();

        /// <summary>A building is being placed: presses only move it, so none is picked until ResumePicking.</summary>
        public Signal SuspendPicking = new();

        /// <summary>The placement is over: presses are picked again.</summary>
        public Signal ResumePicking = new();

        /// <summary>A strike landed on this, for this much damage: its health drops, and at 0 it leaves the board.</summary>
        public Signal<CellOccupantVO, int> DamageOccupant = new();
    }

    /// <summary>
    /// What a press on the board landed on - exactly one of the first three per press - what a secondary press
    /// ordered, and what happened to something on the board that was struck.
    /// </summary>
    public class GridSignalsOutgoing
    {
        /// <summary>A building was pressed: the one standing on the pressed cell, as the grid holds it.</summary>
        public Signal<BoardBuildingVO> BuildingPressed = new();

        /// <summary>A unit was pressed: the one holding the pressed cell, as the grid holds it.</summary>
        public Signal<BoardUnitVO> UnitPressed = new();

        /// <summary>A free cell, or somewhere off the grid, was pressed.</summary>
        public Signal EmptyPressed = new();

        /// <summary>The secondary button was pressed on this free cell of the grid - an order to go there.</summary>
        public Signal<Vector2Int> FreeCellSecondaryPressed = new();

        /// <summary>The secondary button was pressed on a building or a unit - an order to attack it.</summary>
        public Signal<CellOccupantVO> OccupantSecondaryPressed = new();

        /// <summary>This building was struck and still stands; its health is on it.</summary>
        public Signal<BoardBuildingVO> BuildingDamaged = new();

        /// <summary>This building's health reached 0: it is off the board, its cells free.</summary>
        public Signal<BoardBuildingVO> BuildingDestroyed = new();

        /// <summary>This unit was struck and still stands; its health is on it.</summary>
        public Signal<BoardUnitVO> UnitDamaged = new();

        /// <summary>This unit's health reached 0: it is off the board, its cell free.</summary>
        public Signal<BoardUnitVO> UnitDestroyed = new();
    }
}
