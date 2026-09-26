using FlowIoC.BaseModule.Controller;
using FlowIoC.BaseModule.Injectable.Attributes;
using FlowIoC.ConsoleModule;
using Modules.CameraModule.Models;
using UnityEngine;

namespace Modules.CameraModule.Controllers
{
    internal class SetCameraFocusCommand : Command
    {
        [Inject]      private ICameraModel _cameraModel { get; set; }
        [SignalParam] private Rect         _bounds      { get; set; }

        public override void Execute()
        {
            Retain();

            if (_cameraModel.SetFocus(_bounds))
            {
                Release();
                return;
            }

            FlowLogger.LogError($"SetCameraFocusCommand - {_bounds} has no area; the camera keeps its focus.");
            Stop();
        }
    }
}
