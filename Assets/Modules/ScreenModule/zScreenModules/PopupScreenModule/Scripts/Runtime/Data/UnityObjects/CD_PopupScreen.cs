using UnityEngine;

namespace Modules.ScreenModule.PopupScreenModule.Data.UnityObjects
{
    /// <summary>How the popup looks and moves. Filed in the Scriptables of ScreenRoot's RootAdapter - the Root its context is listed on.</summary>
    [CreateAssetMenu(fileName = "CD_PopupScreen", menuName = "Game/Data/CD_PopupScreen")]
    public class CD_PopupScreen : ScriptableObject
    {
        [Tooltip("The sign above the message. Set on the screen as it opens, so the addressable prefab references no atlas sprite.")]
        public Sprite Icon;

        [Tooltip("Seconds the panel takes to pop in. The popup takes clicks once it has.")]
        [Min(0f)] public float PopDuration = 0.18f;

        [Tooltip("The panel's scale when it starts popping in.")]
        [Range(0f, 1f)] public float PopFromScale = 0.85f;
    }
}
