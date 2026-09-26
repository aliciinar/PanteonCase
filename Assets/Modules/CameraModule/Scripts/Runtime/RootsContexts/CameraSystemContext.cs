using FlowIoC.BaseModule.Contexts;
using Modules.CameraModule.Controllers;
using Modules.CameraModule.Models;
using Modules.CameraModule.Signals;
using Modules.CameraModule.ViewsMediators;

namespace Modules.CameraModule.RootsContexts
{

    public class CameraSystemContext : Context
    {
		private CameraSignals _signals;

		private CameraInternalSignals _internalSignals;

        public override void SignalBindings()
        {
            base.SignalBindings();
			_internalSignals = InjectionBinder.Bind<CameraInternalSignals>();
			_signals = InjectionBinderCrossContext.Bind<CameraSignals>();
        }

        public override void InjectionBindings()
        {
            base.InjectionBindings();
            InjectionBinder.Bind<ICameraModel, CameraModel>();
        }

        public override void MediationBindings()
        {
            base.MediationBindings();
            MediationBinder.Bind<CameraView>().To<CameraMediator>();
        }

        public override void CommandBindings()
        {
            base.CommandBindings();

            CommandBinder.Bind(_signals.Incoming.SetViewport)
                .ToSequence<SetCameraViewportCommand>()
                .ToSequence<FitCameraCommand>();

            CommandBinder.Bind(_signals.Incoming.FitToBounds)
                .ToSequence<SetCameraFocusCommand>()
                .ToSequence<FitCameraCommand>();

            CommandBinder.Bind(_signals.Incoming.ScreenResized).ToSequence<FitCameraCommand>();
        }

        public override void Setup()
        {
            base.Setup();
        }

        public override void Launch()
        {
            base.Launch();
        }
    }
}
