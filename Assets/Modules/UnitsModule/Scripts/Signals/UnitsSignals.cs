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
    }

    public class UnitsSignalsOutgoing
    {
        /// <summary>A unit of this type was asked for, but no cell of the board is free.</summary>
        public Signal<UnitType> NoRoomForUnit = new();
    }
}
