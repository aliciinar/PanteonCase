using System.Collections.Generic;
using Modules.UnitsModule.Shared.Enums;
using UnityEngine;

namespace Modules.UnitsModule.Data.ValueObjects
{
    /// <summary>
    /// A unit and the cells it walks through: the first is where it appears, the last where it stops. A
    /// single cell means it stays where it appeared. Handed from step to step of a spawn.
    /// </summary>
    public class UnitMoveVO
    {
        public UnitType Type { get; }
        public IReadOnlyList<Vector2Int> Path { get; }

        public UnitMoveVO(UnitType type, IReadOnlyList<Vector2Int> path)
        {
            Type = type;
            Path = path;
        }
    }
}
