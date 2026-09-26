using FlowIoC.BaseModule.Controller;
using FlowIoC.BaseModule.Injectable.Attributes;
using FlowIoC.ConsoleModule;
using Modules.CameraModule.Models;

namespace Modules.CameraModule.Controllers
{
    internal class SetCameraRightInsetCommand : Command
    {
        [Inject]      private ICameraModel _cameraModel { get; set; }
        [SignalParam] private float        _right       { get; set; }

        public override void Execute()
        {
            Retain();

            if (_cameraModel.SetRightInset(_right))
            {
                Release();
                return;
            }

            FlowLogger.LogError($"SetCameraRightInsetCommand - a right inset of {_right} leaves no area on screen; the camera keeps its viewport.");
            Stop();
        }
    }
}
