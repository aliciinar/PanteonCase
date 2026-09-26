using FlowIoC.BaseModule.Injectable.Components;
using FlowIoC.ScreenModule.ViewsMediators.Screen;
using UnityEngine;

namespace Modules.GameplayModule.InformationScreenModule.ViewsMediators
{
    /// <summary>The information panel on the right of the screen. Measures the screen area it covers.</summary>
    [RequireComponent(typeof(ViewInjector))]
    public class InformationScreenView : ScreenView
    {
        [SerializeField] private RectTransform _panel;

        private readonly Vector3[] _corners = new Vector3[4];

        /// <summary>The panel's area in normalised screen coordinates (0-1, origin bottom-left).</summary>
        public Rect MeasureArea()
        {
            Canvas canvas = _panel.GetComponentInParent<Canvas>().rootCanvas;
            Camera canvasCamera = canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera;

            _panel.GetWorldCorners(_corners);
            Vector2 bottomLeft = RectTransformUtility.WorldToScreenPoint(canvasCamera, _corners[0]);
            Vector2 topRight = RectTransformUtility.WorldToScreenPoint(canvasCamera, _corners[2]);

            return Rect.MinMaxRect(bottomLeft.x / Screen.width, bottomLeft.y / Screen.height,
                                   topRight.x / Screen.width, topRight.y / Screen.height);
        }

        /// <summary>
        /// This method runs if screenData.HasShowAnimation bool is true.
        /// If you don't use custom animations delete this method.
        /// </summary>
        protected override void PlayShowAnimation()
        {
            ShowCompleted?.Invoke(this);
        }

        /// <summary>
        /// This method runs if screenData.HasHideAnimation bool is true.
        /// If you don't use custom animations delete this method.
        /// </summary>
        protected override void PlayHideAnimation()
        {
            HideCompleted?.Invoke(this);
        }
    }
}
