using Modules.GridModule.Shared.Entities;
using Modules.GridModule.Shared.Enums;
using Modules.UnitsModule.Shared.Enums;
using UnityEngine;

namespace Modules.GridModule.Shared.Data.ValueObjects
{
    /// <summary>
    /// A unit on the board: which unit it is, its health, and the cell it holds - where it stands, or where it is
    /// walking to. The cell changes only when the grid moves the unit (IGridService.MoveOccupant).
    /// </summary>
    public class BoardUnitVO : CellOccupantVO
    {
        public override CellOccupantType Kind => CellOccupantType.Soldier;

        public UnitType Type { get; }

        /// <summary>The cell the unit holds in the grid - no other unit is sent there.</summary>
        public Vector2Int Cell { get; internal set; }

        public override RectInt Area => new(Cell, Vector2Int.one);

        public BoardUnitVO(UnitType type, int maxHp, Vector2Int cell, IOccupantView view) : base(maxHp, view)
        {
            Type = type;
            Cell = cell;
        }

        public override string ToString() => $"{Type} {Hp}";
    }
}
