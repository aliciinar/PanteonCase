using Modules.UnitsModule.Shared.Enums;
using UnityEngine;

namespace Modules.UnitsModule.Shared.Data.ValueObjects
{
    /// <summary>
    /// A unit asked of a building: which unit, the cell of the building it comes out of and the cell it
    /// walks to. Cells are the grid's, not relative to the building.
    /// </summary>
    public class UnitSpawnRequestVO
    {
        public UnitType Type { get; }

        /// <summary>The cell of the building the unit comes out of - its door. A cell of the building, so taken.</summary>
        public Vector2Int ExitCell { get; }

        /// <summary>The cell the unit walks to once it is out. May be taken, or off the grid.</summary>
        public Vector2Int SpawnCell { get; }

        public UnitSpawnRequestVO(UnitType type, Vector2Int exitCell, Vector2Int spawnCell)
        {
            Type = type;
            ExitCell = exitCell;
            SpawnCell = spawnCell;
        }
    }
}
