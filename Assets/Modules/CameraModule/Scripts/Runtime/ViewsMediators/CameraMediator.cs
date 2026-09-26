using FlowIoC.BaseModule.Injectable.Attributes;
using FlowIoC.BaseModule.ViewsMediators.Mediator;
using Modules.CameraModule.Data.ValueObjects;
using Modules.CameraModule.Signals;

namespace Modules.CameraModule.ViewsMediators
{
    public class CameraMediator : IMediator
    {
        [Inject]       private CameraView            _view            { get; set; }
        [InjectSignal] private CameraInternalSignals _internalSignals { get; set; }

        public void OnRegister() => _internalSignals.ApplyFit.AddListener(OnApplyFit);

        public void OnRemove() => _internalSignals.ApplyFit.RemoveListener(OnApplyFit);

        private void OnApplyFit(CameraFitVO fit) =>
            _view.Apply(fit.Viewport, fit.HasFocus, fit.Center, fit.OrthographicSize);
    }
}
