using UnityEngine;

namespace Modules.BuildingsModule.Data.ValueObjects
{
    /// <summary>
    /// What the placement preview shows while a placement waits for the player: the building's sprite
    /// over the world rect it would cover, whether it fits there, where the confirm / cancel prompt is
    /// centred, and the cell size the prompt is scaled to.
    /// </summary>
    internal readonly struct PlacementPreviewVO
    {
        public readonly Sprite Sprite;
        public readonly Rect Area;
        public readonly Vector2 PromptCentre;
        public readonly bool Fits;
        public readonly float CellSize;

        public PlacementPreviewVO(Sprite sprite, Rect area, Vector2 promptCentre, bool fits, float cellSize)
        {
            Sprite = sprite;
            Area = area;
            PromptCentre = promptCentre;
            Fits = fits;
            CellSize = cellSize;
        }
    }
}
