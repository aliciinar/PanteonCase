using UnityEngine;

namespace Modules.GameBoardModule.Data.ValueObjects
{
    /// <summary>
    /// What the board view shows while a placement waits for the player: the building's sprite over the
    /// world rect it would cover, whether it fits there, and where the confirm / cancel prompt is centred.
    /// </summary>
    internal readonly struct PlacementPreviewVO
    {
        public readonly Sprite Sprite;
        public readonly Rect Area;
        public readonly Vector2 PromptCentre;
        public readonly bool Fits;

        public PlacementPreviewVO(Sprite sprite, Rect area, Vector2 promptCentre, bool fits)
        {
            Sprite = sprite;
            Area = area;
            PromptCentre = promptCentre;
            Fits = fits;
        }
    }
}
