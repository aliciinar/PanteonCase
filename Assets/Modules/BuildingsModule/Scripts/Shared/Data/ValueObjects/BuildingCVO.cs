using System;
using UnityEngine;

namespace Modules.BuildingsModule.Shared.Data.ValueObjects
{
    /// <summary>One building as the designer authors it: how the menu shows it, how the board shows it, and its footprint.</summary>
    [Serializable]
    public class BuildingCVO
    {
        [Tooltip("The building's card image in the production menu.")]
        public Sprite Icon;

        [Tooltip("Drawn over the building's footprint on the board, fitted to it whatever the sprite's own size.")]
        public Sprite BoardSprite;

        [Tooltip("Footprint in cells: columns (x) and rows (y).")]
        [Min(1)] public Vector2Int Size = Vector2Int.one;
    }
}
