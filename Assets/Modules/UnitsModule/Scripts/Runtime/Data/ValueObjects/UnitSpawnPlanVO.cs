using System.Collections.Generic;
using Modules.UnitsModule.Shared.Enums;
using UnityEngine;

namespace Modules.UnitsModule.Data.ValueObjects
{
    /// <summary>
    /// A unit to put on the board and the cells it walks through: the first is its building's door, where it appears,
    /// the last its goal, where it stops and which it holds. A single cell means it appears on the goal itself.
    /// </summary>
    public class UnitSpawnPlanVO
    {
        public UnitType Type { get; }
        public IReadOnlyList<Vector2Int> Path { get; }

        public UnitSpawnPlanVO(UnitType type, IReadOnlyList<Vector2Int> path)
        {
            Type = type;
            Path = path;
        }
    }
}
