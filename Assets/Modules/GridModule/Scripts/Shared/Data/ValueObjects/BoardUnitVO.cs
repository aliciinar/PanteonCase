using Modules.GridModule.Shared.Enums;
using Modules.UnitsModule.Shared.Enums;

namespace Modules.GridModule.Shared.Data.ValueObjects
{
    /// <summary>A unit standing on the board - or walking to the cell it holds: which unit it is and the health it has left.</summary>
    public class BoardUnitVO : CellOccupantVO
    {
        public override CellOccupantType Kind => CellOccupantType.Soldier;

        public UnitType Type { get; }

        public int Hp { get; set; }

        public BoardUnitVO(UnitType type, int hp)
        {
            Type = type;
            Hp = hp;
        }

        public override string ToString() => $"{Type} {Hp}";
    }
}
