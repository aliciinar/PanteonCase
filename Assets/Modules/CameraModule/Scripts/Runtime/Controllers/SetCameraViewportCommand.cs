using FlowIoC.BaseModule.Controller;
using FlowIoC.BaseModule.Injectable.Attributes;
using FlowIoC.ConsoleModule;
using Modules.CameraModule.Models;
using UnityEngine;

namespace Modules.CameraModule.Controllers
{
    internal class SetCameraViewportCommand : Command
    {
        [Inject]      private ICameraModel _cameraModel { get; set; }
        [SignalParam] private Rect         _viewport    { get; set; }

        public override void Execute()
        {
            if (_cameraModel.SetViewport(_viewport)) return;

            FlowLogger.LogError($"SetCameraViewportCommand - {_viewport} leaves no area on screen; the camera keeps its viewport.");
            Stop();
        }
    }
}
