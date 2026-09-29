using UnityEngine;

namespace Modules.BuildingsModule.Data.ValueObjects
{
    /// <summary>
    /// What the placement preview shows while a placement waits for the player: the building's sprite
    /// over the world rect it would cover, whether it fits there and the ghost's colour for that, where the
    /// confirm / cancel prompt is centred, the cell size the prompt is scaled to, and the building's name
    /// and footprint in cells, written under the prompt.
    /// </summary>
    internal readonly struct PlacementPreviewVO
    {
        public readonly Sprite Sprite;
        public readonly Rect Area;
        public readonly Vector2 PromptCentre;
        public readonly bool Fits;
        public readonly Color GhostColor;
        public readonly float CellSize;
        public readonly string Name;
        public readonly Vector2Int Size;

        public PlacementPreviewVO(Sprite sprite, Rect area, Vector2 promptCentre, bool fits, Color color, float cellSize,
                                  string name, Vector2Int size)
        {
            Sprite = sprite;
            Area = area;
            PromptCentre = promptCentre;
            Fits = fits;
            GhostColor = color;
            CellSize = cellSize;
            Name = name;
            Size = size;
        }
    }
}
