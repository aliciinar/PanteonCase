using Modules.BuildingsModule.Entities;
using Modules.BuildingsModule.Shared.Enums;
using UnityEngine;

namespace Modules.BuildingsModule.Data.ValueObjects
{
    /// <summary>
    /// A placed building the buildings view puts on the board: the pooled object, which building it is,
    /// its sprite and the world rect it covers.
    /// </summary>
    internal readonly struct PlacedBuildingVO
    {
        public readonly BoardBuilding Building;
        public readonly BuildType Type;
        public readonly Sprite Sprite;
        public readonly Rect Area;

        public PlacedBuildingVO(BoardBuilding building, BuildType type, Sprite sprite, Rect area)
        {
            Building = building;
            Type = type;
            Sprite = sprite;
            Area = area;
        }
    }
}
