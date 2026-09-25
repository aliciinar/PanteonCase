using System;
using FlowIoC.BaseModule.Injectable.Components;
using FlowIoC.ScreenModule.ViewsMediators.Screen;
using UnityEngine;

namespace Modules.GameplayModule.GameplayScreenModule.ViewsMediators
{
    /// <summary>
    /// The gameplay HUD: the production panel on the left, the information panel on the right, and
    /// between them an empty PlayArea rect the game world shows through. The view measures that rect
    /// and reports it; it does not know who draws into it.
    /// </summary>
    [RequireComponent(typeof(ViewInjector))]
    public class GameplayScreenView : ScreenView
    {
        [SerializeField] private RectTransform _playArea;

        /// <summary>The play area in normalised screen coordinates (0-1, origin bottom-left).</summary>
        public Action<Rect> PlayAreaChanged;

        private readonly Vector3[] _corners = new Vector3[4];

        private bool _isPlayAreaDirty;
        private Vector2Int _lastScreenSize;

        /// <summary>Measures the play area and raises PlayAreaChanged with it.</summary>
        public void ReportPlayArea()
        {
            _isPlayAreaDirty = false;

            // A minimised window reports a zero-sized screen; there is no area to report then.
            if (Screen.width <= 0 || Screen.height <= 0) return;

            Canvas canvas = _playArea.GetComponentInParent<Canvas>().rootCanvas;
            Camera canvasCamera = canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera;

            _playArea.GetWorldCorners(_corners);
            Vector2 bottomLeft = RectTransformUtility.WorldToScreenPoint(canvasCamera, _corners[0]);
            Vector2 topRight = RectTransformUtility.WorldToScreenPoint(canvasCamera, _corners[2]);

            PlayAreaChanged?.Invoke(Rect.MinMaxRect(bottomLeft.x / Screen.width, bottomLeft.y / Screen.height,
                                                    topRight.x / Screen.width, topRight.y / Screen.height));
        }

        // A resize reaches this rect before the CanvasScaler has rescaled the canvas for the new
        // window, so measuring here would report the old layout. Mark it and measure in LateUpdate,
        // after every Update - the scaler's included - has run for the frame.
        private void OnRectTransformDimensionsChange() => _isPlayAreaDirty = true;

        private void LateUpdate()
        {
            var screenSize = new Vector2Int(Screen.width, Screen.height);
            if (screenSize != _lastScreenSize)
            {
                _lastScreenSize = screenSize;
                _isPlayAreaDirty = true;
            }

            if (_isPlayAreaDirty) ReportPlayArea();
        }

        /// <summary>
        /// This method runs if screenData.HasShowAnimation bool is true.
        /// If you don't use custom animations delete this method.
        /// </summary>
        protected override void PlayShowAnimation()
        {
            // Do some animation
            ShowCompleted?.Invoke(this);
        }

        /// <summary>
        /// This method runs if screenData.HasHideAnimation bool is true.
        /// If you don't use custom animations delete this method.
        /// </summary>
        protected override void PlayHideAnimation()
        {
            // Do some animation
            HideCompleted?.Invoke(this);
        }
    }
}
