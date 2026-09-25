using UnityEngine;

namespace Modules.CameraModule.Data.ValueObjects
{
    /// <summary>Where the camera draws and what it looks at. Computed by FitCameraCommand.</summary>
    internal readonly struct CameraFitVO
    {
        /// <summary>Normalised screen rect the camera renders into.</summary>
        public readonly Rect Viewport;

        /// <summary>False until a FitToBounds has arrived; the view then only applies the viewport.</summary>
        public readonly bool HasFocus;

        /// <summary>World point the camera centres on.</summary>
        public readonly Vector2 Center;

        public readonly float OrthographicSize;

        public CameraFitVO(Rect viewport, bool hasFocus, Vector2 center, float orthographicSize)
        {
            Viewport = viewport;
            HasFocus = hasFocus;
            Center = center;
            OrthographicSize = orthographicSize;
        }
    }
}
