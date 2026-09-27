using Modules.BuildingsModule.Shared.Enums;
using UnityEngine;

namespace Modules.BuildingsModule.Data.ValueObjects
{
    /// <summary>
    /// A building standing on the board: the entity id the grid gave its cells, which building it is,
    /// the cells it covers and the health it has left.
    /// </summary>
    internal class BuildingRecordVO
    {
        /// <summary>The entity id every cell of the building holds in the grid.</summary>
        public int Id { get; }

        public BuildType Type { get; }

        /// <summary>The cells the building covers: bottom-left cell and size.</summary>
        public RectInt Area { get; }

        public int Hp { get; set; }

        public BuildingRecordVO(int id, BuildType type, RectInt area, int hp)
        {
            Id = id;
            Type = type;
            Area = area;
            Hp = hp;
        }
    }
}
