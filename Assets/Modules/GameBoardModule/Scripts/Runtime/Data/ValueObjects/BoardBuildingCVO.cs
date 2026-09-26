using System;
using UnityEngine;

namespace Modules.GameBoardModule.Data.ValueObjects
{
    /// <summary>How a building stands on the board: the cells it covers and the sprite drawn over them.</summary>
    [Serializable]
    public class BoardBuildingCVO
    {
        [Tooltip("Drawn over the building's footprint, fitted to it whatever the sprite's own size.")]
        public Sprite Sprite;

        [Tooltip("Footprint in cells: columns (x) and rows (y).")]
        [Min(1)] public Vector2Int Size = Vector2Int.one;
    }
}
