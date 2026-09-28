namespace Modules.ScreenModule.PopupScreenModule.Data.ValueObjects
{
    /// <summary>How the popup pops in, as CD_PopupScreen authors it - handed to the screen as it opens.</summary>
    internal readonly struct PopupAnimationVO
    {
        public readonly float Duration;
        public readonly float FromScale;

        public PopupAnimationVO(float duration, float fromScale)
        {
            Duration = duration;
            FromScale = fromScale;
        }
    }
}
