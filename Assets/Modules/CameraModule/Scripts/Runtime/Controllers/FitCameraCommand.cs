using FlowIoC.BaseModule.Controller;
using FlowIoC.BaseModule.Injectable.Attributes;
using Modules.CameraModule.Data.ValueObjects;
using Modules.CameraModule.Models;
using Modules.CameraModule.Signals;
using UnityEngine;

namespace Modules.CameraModule.Controllers
{
    /// <summary>
    /// Sizes the orthographic camera so the focus rect fits the viewport on both axes, centred. The
    /// viewport's aspect is measured in pixels, so the result follows the window as it is resized.
    /// </summary>
    internal class FitCameraCommand : Command
    {
        [Inject]       private ICameraModel          _cameraModel     { get; set; }
        [InjectSignal] private CameraInternalSignals _internalSignals { get; set; }

        public override void Execute()
        {
            Rect viewport = _cameraModel.Viewport;

            if (!_cameraModel.HasFocus)
            {
                _internalSignals.ApplyFit.Dispatch(new CameraFitVO(viewport, false, Vector2.zero, 0f));
                return;
            }

            Rect focus = _cameraModel.Focus;
            float viewportAspect = viewport.width * Screen.width / Mathf.Max(1f, viewport.height * Screen.height);

            // Orthographic size is half the visible height. Fit the height, then widen it if the focus
            // is relatively wider than the viewport.
            float sizeForHeight = focus.height * 0.5f;
            float sizeForWidth = focus.width * 0.5f / Mathf.Max(viewportAspect, Mathf.Epsilon);

            _internalSignals.ApplyFit.Dispatch(new CameraFitVO(viewport, true, focus.center,
                                                               Mathf.Max(sizeForHeight, sizeForWidth)));
        }
    }
}
