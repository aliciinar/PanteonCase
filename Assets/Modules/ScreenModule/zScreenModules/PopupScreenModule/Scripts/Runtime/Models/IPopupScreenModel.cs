using Modules.ScreenModule.PopupScreenModule.Data.ValueObjects;

namespace Modules.ScreenModule.PopupScreenModule.Models
{
    /// <summary>The popup as CD_PopupScreen authors it.</summary>
    internal interface IPopupScreenModel
    {
        /// <summary>How the panel pops in.</summary>
        PopupAnimationVO Animation { get; }
    }
}
