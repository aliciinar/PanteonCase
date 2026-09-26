using Modules.BuildingsModule.Shared.Enums;
using Modules.GameBoardModule.Entities;
using UnityEngine;

namespace Modules.GameBoardModule.Data.ValueObjects
{
    /// <summary>
    /// A building the board view puts on the board: the pooled object, which building it is, its sprite
    /// and the world rect it covers.
    /// </summary>
    internal readonly struct BoardBuildingVO
    {
        public readonly BoardBuilding Building;
        public readonly BuildType Type;
        public readonly Sprite Sprite;
        public readonly Rect Area;

        public BoardBuildingVO(BoardBuilding building, BuildType type, Sprite sprite, Rect area)
        {
            Building = building;
            Type = type;
            Sprite = sprite;
            Area = area;
        }
    }
}
