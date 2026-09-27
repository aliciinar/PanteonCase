using Modules.BuildingsModule.Shared.Enums;
using Modules.GridModule.Shared.Entities;
using Modules.GridModule.Shared.Enums;
using UnityEngine;

namespace Modules.GridModule.Shared.Data.ValueObjects
{
    /// <summary>A building standing on the board: which building it is, the cells it covers, its health and the object that shows it.</summary>
    public class BoardBuildingVO : CellOccupantVO
    {
        public override CellOccupantType Kind => CellOccupantType.Building;

        public BuildType Type { get; }

        public override RectInt Area { get; }

        public BoardBuildingVO(BuildType type, RectInt area, int maxHp, IOccupantView view) : base(maxHp, view)
        {
            Type = type;
            Area = area;
        }

        public override string ToString() => $"{Type} {Hp}";
    }
}
