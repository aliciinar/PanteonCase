using Modules.BuildingsModule.Shared.Enums;
using UnityEngine;

namespace Modules.BuildingsModule.Shared.Data.ValueObjects
{
    /// <summary>
    /// A building standing on the board, as a screen shows it: which building it is, the health it has
    /// left, and - for a building that produces units - the grid cells its units come out of (its door)
    /// and walk to (its spawn point). Everything else a screen shows of it - its image, its full health,
    /// what it produces - is authored in CD_Buildings and read there.
    /// </summary>
    public class BuildingInfoVO
    {
        public BuildType Type { get; }

        public int Hp { get; }

        /// <summary>The grid cell of the building its units come out of - its door.</summary>
        public Vector2Int ExitCell { get; }

        /// <summary>The grid cell outside the building its units walk to.</summary>
        public Vector2Int SpawnCell { get; }

        public BuildingInfoVO(BuildType type, int hp, Vector2Int exitCell, Vector2Int spawnCell)
        {
            Type = type;
            Hp = hp;
            ExitCell = exitCell;
            SpawnCell = spawnCell;
        }
    }
}
