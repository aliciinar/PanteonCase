using FlowIoC.BaseModule.Controller;
using FlowIoC.BaseModule.Injectable.Attributes;
using FlowIoC.ConsoleModule;
using Modules.CameraModule.Models;

namespace Modules.CameraModule.Controllers
{
    internal class SetCameraLeftInsetCommand : Command
    {
        [Inject]      private ICameraModel _cameraModel { get; set; }
        [SignalParam] private float        _left        { get; set; }

        public override void Execute()
        {
            if (_cameraModel.SetLeftInset(_left)) return;

            FlowLogger.LogError($"SetCameraLeftInsetCommand - a left inset of {_left} leaves no area on screen; the camera keeps its viewport.");
            Stop();
        }
    }
}
