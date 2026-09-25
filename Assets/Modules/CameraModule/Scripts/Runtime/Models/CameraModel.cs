using UnityEngine;

namespace Modules.CameraModule.Models
{
    public class CameraModel : ICameraModel
    {
        private static readonly Rect FullScreen = new(0f, 0f, 1f, 1f);

        public Rect Viewport { get; private set; } = FullScreen;
        public bool HasFocus { get; private set; }
        public Rect Focus { get; private set; }

        public bool SetViewport(Rect normalizedViewport)
        {
            float xMin = Mathf.Clamp01(normalizedViewport.xMin);
            float yMin = Mathf.Clamp01(normalizedViewport.yMin);
            float xMax = Mathf.Clamp01(normalizedViewport.xMax);
            float yMax = Mathf.Clamp01(normalizedViewport.yMax);

            if (xMax - xMin <= 0f || yMax - yMin <= 0f) return false;

            Viewport = Rect.MinMaxRect(xMin, yMin, xMax, yMax);
            return true;
        }

        public bool SetFocus(Rect worldBounds)
        {
            if (worldBounds.width <= 0f || worldBounds.height <= 0f) return false;

            Focus = worldBounds;
            HasFocus = true;
            return true;
        }
    }
}
