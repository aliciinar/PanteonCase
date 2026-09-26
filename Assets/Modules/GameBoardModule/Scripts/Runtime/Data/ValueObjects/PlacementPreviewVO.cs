using UnityEngine;

namespace Modules.GameBoardModule.Data.ValueObjects
{
    /// <summary>
    /// What the board view shows while a placement waits for the player: the building's sprite over the
    /// world rect it would cover, and where the confirm / cancel prompt is centred.
    /// </summary>
    internal readonly struct PlacementPreviewVO
    {
        public readonly Sprite Sprite;
        public readonly Rect Area;
        public readonly Vector2 PromptCentre;

        public PlacementPreviewVO(Sprite sprite, Rect area, Vector2 promptCentre)
        {
            Sprite = sprite;
            Area = area;
            PromptCentre = promptCentre;
        }
    }
}
