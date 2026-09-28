using FlowIoC.BaseModule.Signals;
using Modules.GridModule.Shared.Data.ValueObjects;
using Modules.UnitsModule.Shared.Data.ValueObjects;
using Modules.UnitsModule.Shared.Enums;
using UnityEngine;

namespace Modules.UnitsModule.Signals
{
    public class UnitsSignals : ISignalHolder
    {
        public UnitsSignalsIncoming Incoming = new();
        public UnitsSignalsOutgoing Outgoing = new();
    }

    public class UnitsSignalsIncoming
    {
        /// <summary>
        /// Put a unit on the board for a building: it comes out of the building's exit cell - its door -
        /// and walks by A* to the spawn cell, or, when that is taken or off the grid, the free cell nearest
        /// it. Answered with NoRoomForUnit when no cell is free.
        /// </summary>
        public Signal<UnitSpawnRequestVO> SpawnUnit = new();

        /// <summary>A unit on the board was pressed: it is the selected one from now on.</summary>
        public Signal<BoardUnitVO> SelectUnit = new();

        /// <summary>Something other than a unit was pressed: no unit is selected.</summary>
        public Signal ClearSelection = new();

        /// <summary>A free cell was ordered: the selected unit, if any, walks there by A*.</summary>
        public Signal<Vector2Int> MoveSelectedUnit = new();

        /// <summary>A building or a unit was ordered attacked: the selected unit, if any, walks up to it and strikes once.</summary>
        public Signal<CellOccupantVO> AttackWithSelectedUnit = new();

        /// <summary>This unit was struck and still stands - its health is on it: show the hit.</summary>
        public Signal<BoardUnitVO> UnitDamaged = new();

        /// <summary>This unit was destroyed and is off the board: put it away.</summary>
        public Signal<BoardUnitVO> RemoveUnit = new();
    }

    public class UnitsSignalsOutgoing
    {
        /// <summary>A unit of this type was asked for, but no cell of the board is free.</summary>
        public Signal<UnitType> NoRoomForUnit = new();

        /// <summary>
        /// The selected unit could not carry out an order - no free cell next to the target, no way there. The message,
        /// authored in CD_Units, says why, for the player.
        /// </summary>
        public Signal<string> OrderRefused = new();

        /// <summary>A unit was selected: which one and its health now.</summary>
        public Signal<UnitInfoVO> UnitSelected = new();

        /// <summary>The selected unit is no longer selected - another press cleared it, or it was destroyed.</summary>
        public Signal SelectionCleared = new();

        /// <summary>A strike landed on this, for this much damage.</summary>
        public Signal<CellOccupantVO, int> AttackLanded = new();

        /// <summary>A unit started an action - walking out of its door, walking to a cell, or walking up to strike.</summary>
        public Signal ActionStarted = new();

        /// <summary>The action is over.</summary>
        public Signal ActionEnded = new();
    }
}
