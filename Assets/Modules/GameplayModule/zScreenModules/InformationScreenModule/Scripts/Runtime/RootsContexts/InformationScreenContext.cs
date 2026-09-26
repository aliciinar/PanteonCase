using FlowIoC.ScreenModule.Data;
using FlowIoC.ScreenModule.Enums;
using FlowIoC.ScreenModule.RootsContexts;
using Modules.GameplayModule.InformationScreenModule.Controllers;
using Modules.GameplayModule.InformationScreenModule.Signals;
using Modules.GameplayModule.InformationScreenModule.ViewsMediators;

namespace Modules.GameplayModule.InformationScreenModule.RootsContexts
{
    public class InformationScreenContext : ScreenSubContext<InformationScreenView, InformationScreenMediator>
    {
        protected override ScreenCVO Screen => new()
        {
            ManagerId = 0,
            Layer = 2,
            Tag = ScreenTag.Default,
            Load = ScreenLoadCVO.Addressable("InformationScreen"),
            HasShowAnimation = false,
            HasHideAnimation = false,
        };

		private InformationScreenSignals _signals;

        public override void SignalBindings()
        {
            base.SignalBindings();
			_signals = InjectionBinderCrossContext.Bind<InformationScreenSignals>();
        }

        public override void CommandBindings()
        {
            base.CommandBindings();

            // Opening reports the panel's area; so does a resize, once the UI has rescaled.
            CommandBinder.Bind(_signals.Incoming.OpenInformationScreen)
                .ToSequence<OpenInformationScreenCommand>()
                .ToSequence<ReportInformationAreaCommand>();

            CommandBinder.Bind(_signals.Incoming.ScreenResized)
                .ToSequence<WaitForLayoutCommand>()
                .ToSequence<ReportInformationAreaCommand>();
        }
    }
}
