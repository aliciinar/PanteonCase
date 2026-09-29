using UnityEngine;

namespace Modules.MainModule.Models
{
    /// <summary>The application window's size, set by ApplyScreenSizeCommand.</summary>
    internal interface IScreenResizeModel
    {
        /// <summary>Screen size in pixels, as of the last change.</summary>
        Vector2Int ScreenSize { get; }

        void SetScreenSize(Vector2Int screenSize);
    }
}
