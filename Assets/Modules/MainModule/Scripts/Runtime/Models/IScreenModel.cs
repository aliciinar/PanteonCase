using UnityEngine;

namespace Modules.MainModule.Models
{
    /// <summary>The application window's size, kept current and announced through MainSignals.Outgoing.ScreenResized.</summary>
    public interface IScreenModel
    {
        /// <summary>Screen size in pixels, as of the last change.</summary>
        Vector2Int ScreenSize { get; }
    }
}
