using System.Collections.Generic;
using Modules.GridModule.Shared.Data.ValueObjects;
using UnityEngine;

namespace Modules.UnitsModule.Data.ValueObjects
{
    /// <summary>
    /// An attack as planned: the attacking unit, what it strikes - as the grid holds it - and the cells it walks
    /// through to the cell it strikes from; the first is where it stands, a single cell means it strikes from there.
    /// </summary>
    public class UnitAttackPlanVO
    {
        public BoardUnitVO Attacker { get; }
        public CellOccupantVO Target { get; }
        public IReadOnlyList<Vector2Int> Path { get; }

        public UnitAttackPlanVO(BoardUnitVO attacker, CellOccupantVO target, IReadOnlyList<Vector2Int> path)
        {
            Attacker = attacker;
            Target = target;
            Path = path;
        }
    }
}
