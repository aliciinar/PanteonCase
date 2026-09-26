using Modules.BuildingsModule.Signals;
using FlowIoC.BaseModule.Contexts;

namespace Modules.BuildingsModule.RootsContexts
{

    public class BuildingsSystemContext : Context
    {
		private BuildingsSignals _signals;

		private BuildingsInternalSignals _internalSignals;

        public override void SignalBindings()
        {
            base.SignalBindings();
			_internalSignals = InjectionBinder.Bind<BuildingsInternalSignals>();
			_signals = InjectionBinderCrossContext.Bind<BuildingsSignals>();
        }

        public override void InjectionBindings()
        {
            base.InjectionBindings();
        }

        public override void MediationBindings()
        {
            base.MediationBindings();
        }

        public override void CommandBindings()
        {
            base.CommandBindings();
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
