using System;
using UnityEngine;

namespace Modules.MainModule.Entities
{
    /// <summary>
    /// Sits on an empty root Canvas under MainRoot. Unity resizes a root Canvas's RectTransform to the
    /// screen whenever the window changes size and calls OnRectTransformDimensionsChange for it, so
    /// this is how the window size is watched without polling. Filed on MainRoot's RootAdapter and
    /// read by ScreenModel.
    /// </summary>
    [RequireComponent(typeof(Canvas))]
    public class ScreenResizeNotifier : MonoBehaviour
    {
        /// <summary>The new screen size in pixels.</summary>
        public event Action<Vector2Int> Resized;

        private void OnRectTransformDimensionsChange() =>
            Resized?.Invoke(new Vector2Int(Screen.width, Screen.height));
    }
}
