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

        /// <summary>
        /// The panel's area in normalised screen coordinates (0-1, origin bottom-left), shrunk to the whole pixels
        /// it paints: at a fractional canvas scale an edge falls mid-pixel, the panel leaves that column unpainted,
        /// and a camera viewport laid beside the raw edge would round away from it and leave a black seam.
        /// </summary>
        public Rect MeasureArea()
        {
            Canvas canvas = _panel.GetComponentInParent<Canvas>().rootCanvas;
            Camera canvasCamera = canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera;

            _panel.GetWorldCorners(_corners);
            Vector2 bottomLeft = RectTransformUtility.WorldToScreenPoint(canvasCamera, _corners[0]);
            Vector2 topRight = RectTransformUtility.WorldToScreenPoint(canvasCamera, _corners[2]);

            return Rect.MinMaxRect(Mathf.Ceil(bottomLeft.x) / Screen.width, Mathf.Ceil(bottomLeft.y) / Screen.height,
                                   Mathf.Floor(topRight.x) / Screen.width, Mathf.Floor(topRight.y) / Screen.height);
        }
    }
}
