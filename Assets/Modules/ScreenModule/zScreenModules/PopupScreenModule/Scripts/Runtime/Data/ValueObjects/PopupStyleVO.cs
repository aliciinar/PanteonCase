using UnityEngine;

namespace Modules.ScreenModule.PopupScreenModule.Data.ValueObjects
{
    /// <summary>
    /// How the popup looks and pops in, as CD_PopupScreen authors it - handed to the screen as it opens. The icon comes
    /// from the config rather than the prefab, so the addressable prefab references no atlas sprite and its bundle
    /// carries no copy of the UI atlas.
    /// </summary>
    internal readonly struct PopupStyleVO
    {
        public readonly Sprite Icon;
        public readonly float PopDuration;
        public readonly float PopFromScale;

        public PopupStyleVO(Sprite icon, float popDuration, float popFromScale)
        {
            Icon = icon;
            PopDuration = popDuration;
            PopFromScale = popFromScale;
        }
    }
}
