using UnityEngine;

namespace Modules.CameraModule.Models
{
    /// <summary>What the camera has been told to do: where on screen it draws, and what it keeps in view.</summary>
    public interface ICameraModel
    {
        /// <summary>Normalised screen rect (0-1). The whole screen until told otherwise.</summary>
        Rect Viewport { get; }

        /// <summary>True once a focus rect has been set.</summary>
        bool HasFocus { get; }

        /// <summary>World rect to keep in view.</summary>
        Rect Focus { get; }

        /// <summary>Stores the viewport, clamped to the screen. Returns false and keeps the old one for an empty rect.</summary>
        bool SetViewport(Rect normalizedViewport);

        /// <summary>Stores the focus. Returns false and keeps the old one for an empty rect.</summary>
        bool SetFocus(Rect worldBounds);
    }
}
