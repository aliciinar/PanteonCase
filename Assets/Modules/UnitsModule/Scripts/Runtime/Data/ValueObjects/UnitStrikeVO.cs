using Modules.GridModule.Shared.Data.ValueObjects;

namespace Modules.UnitsModule.Data.ValueObjects
{
    /// <summary>
    /// Who strikes what, as the grid holds them. One value rather than two signal parameters: a unit is also a cell
    /// occupant, so two parameters could not be told apart by type.
    /// </summary>
    internal class UnitStrikeVO
    {
        public BoardUnitVO Attacker { get; }
        public CellOccupantVO Target { get; }

        public UnitStrikeVO(BoardUnitVO attacker, CellOccupantVO target)
        {
            Attacker = attacker;
            Target = target;
        }
    }
}
