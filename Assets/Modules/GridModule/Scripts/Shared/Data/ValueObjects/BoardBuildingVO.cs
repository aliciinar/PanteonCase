using Modules.BuildingsModule.Shared.Enums;
using Modules.GridModule.Shared.Enums;
using UnityEngine;

namespace Modules.GridModule.Shared.Data.ValueObjects
{
    /// <summary>A building standing on the board: which building it is, the cells it covers and the health it has left.</summary>
    public class BoardBuildingVO : CellOccupantVO
    {
        public override CellOccupantType Kind => CellOccupantType.Building;

        public BuildType Type { get; }

        /// <summary>The cells the building covers: bottom-left cell and size.</summary>
        public RectInt Area { get; }

        public int Hp { get; set; }

        public BoardBuildingVO(BuildType type, RectInt area, int hp)
        {
            Type = type;
            Area = area;
            Hp = hp;
        }

        public override string ToString() => $"{Type} {Hp}";
    }
}
