using Modules.BuildingsModule.Shared.Enums;
using UnityEngine;

namespace Modules.GameBoardModule.Data.ValueObjects
{
    /// <summary>A building and the area of the board it goes on, in cells. Handed from step to step of a placement.</summary>
    public class BuildingPlacementVO
    {
        public BuildType Type { get; }

        /// <summary>The cells the building covers: bottom-left cell and size.</summary>
        public RectInt Area { get; }

        public BuildingPlacementVO(BuildType type, RectInt area)
        {
            Type = type;
            Area = area;
        }
    }
}
