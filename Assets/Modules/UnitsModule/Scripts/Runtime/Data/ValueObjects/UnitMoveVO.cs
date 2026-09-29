using System.Collections.Generic;
using Modules.GridModule.Shared.Data.ValueObjects;
using UnityEngine;

namespace Modules.UnitsModule.Data.ValueObjects
{
    /// <summary>
    /// A unit - as the grid holds it - and the cells it walks through: the first is where it starts, the last where it
    /// stops. A single cell means it stays where it is. Handed from step to step of a spawn or of a move order.
    /// </summary>
    internal class UnitMoveVO
    {
        public BoardUnitVO Unit { get; }
        public IReadOnlyList<Vector2Int> Path { get; }

        public UnitMoveVO(BoardUnitVO unit, IReadOnlyList<Vector2Int> path)
        {
            Unit = unit;
            Path = path;
        }
    }
}
