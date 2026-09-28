using Modules.ScreenModule.PopupScreenModule.Data.ValueObjects;

namespace Modules.ScreenModule.PopupScreenModule.Models
{
    /// <summary>The popup as CD_PopupScreen authors it.</summary>
    internal interface IPopupScreenModel
    {
        /// <summary>The popup's icon and how its panel pops in.</summary>
        PopupStyleVO Style { get; }
    }
}
