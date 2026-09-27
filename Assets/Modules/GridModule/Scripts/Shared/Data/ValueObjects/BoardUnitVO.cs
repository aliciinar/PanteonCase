using Modules.GridModule.Shared.Enums;
using Modules.UnitsModule.Shared.Enums;
using UnityEngine;

namespace Modules.GridModule.Shared.Data.ValueObjects
{
    /// <summary>
    /// A unit on the board: which unit it is, the health it has left, the cell it holds - where it stands, or where it
    /// is walking to - and the cell it is on its way through right now. Both change only through the grid
    /// (IGridService.MoveOccupant, IGridService.StepUnit).
    /// </summary>
    public class BoardUnitVO : CellOccupantVO
    {
        public override CellOccupantType Kind => CellOccupantType.Soldier;

        public UnitType Type { get; }

        public int Hp { get; set; }

        /// <summary>The cell the unit holds in the grid - no other unit is sent there.</summary>
        public Vector2Int Cell { get; internal set; }

        /// <summary>
        /// The cell the unit stands on, or is stepping into while it walks. It holds no cell of the grid; it is where a
        /// new walk starts from, so a unit ordered elsewhere turns where it is.
        /// </summary>
        public Vector2Int StepCell { get; internal set; }

        public BoardUnitVO(UnitType type, int hp, Vector2Int cell)
        {
            Type = type;
            Hp = hp;
            Cell = cell;
            StepCell = cell;
        }

        public override string ToString() => $"{Type} {Hp}";
    }
}
