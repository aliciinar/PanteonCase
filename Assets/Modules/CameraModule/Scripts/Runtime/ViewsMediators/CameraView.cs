using System;
using FlowIoC.BaseModule.Injectable.Components;
using FlowIoC.BaseModule.ViewsMediators.View;
using UnityEngine;

namespace Modules.CameraModule.ViewsMediators
{
    /// <summary>The game camera. Applies the fit it is handed and reports when the window changes size.</summary>
    [RequireComponent(typeof(ViewInjector))]
    public class CameraView : MonoBehaviour, IView
    {
        public bool IsRegistered { get; set; }

        [SerializeField] private Camera _camera;

        public Action ScreenResized;

        private Vector2Int _lastScreenSize;

        public void Apply(Rect viewport, bool hasFocus, Vector2 center, float orthographicSize)
        {
            _camera.rect = viewport;

            if (!hasFocus) return;

            Transform cameraTransform = _camera.transform;
            cameraTransform.position = new Vector3(center.x, center.y, cameraTransform.position.z);
            _camera.orthographicSize = orthographicSize;
        }

        private void LateUpdate()
        {
            var screenSize = new Vector2Int(Screen.width, Screen.height);
            if (screenSize == _lastScreenSize) return;

            _lastScreenSize = screenSize;
            ScreenResized?.Invoke();
        }
    }
}
