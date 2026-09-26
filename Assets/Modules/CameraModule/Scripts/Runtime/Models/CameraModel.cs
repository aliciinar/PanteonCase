using UnityEngine;

namespace Modules.CameraModule.Models
{
    public class CameraModel : ICameraModel
    {
        private float _left;
        private float _right = 1f;

        public Rect Viewport => new(_left, 0f, _right - _left, 1f);
        public bool HasFocus { get; private set; }
        public Rect Focus { get; private set; }

        public bool SetLeftInset(float normalizedX)
        {
            float left = Mathf.Clamp01(normalizedX);
            if (left >= _right) return false;

            _left = left;
            return true;
        }

        public bool SetRightInset(float normalizedX)
        {
            float right = Mathf.Clamp01(normalizedX);
            if (right <= _left) return false;

            _right = right;
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
