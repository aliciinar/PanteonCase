using FlowIoC.ScreenModule.Data;
using FlowIoC.ScreenModule.Enums;
using FlowIoC.ScreenModule.RootsContexts;
using Modules.GameplayModule.InfoScreenModule.Controllers;
using Modules.GameplayModule.InfoScreenModule.Enums;
using Modules.GameplayModule.InfoScreenModule.Models;
using Modules.GameplayModule.InfoScreenModule.Signals;
using Modules.GameplayModule.InfoScreenModule.ViewsMediators;

namespace Modules.GameplayModule.InfoScreenModule.RootsContexts
{
    public class InfoScreenContext : ScreenSubContext<InfoScreenView, InfoScreenMediator>
    {
        // One layer above the information panel (2), inside which it is drawn.
        protected override ScreenCVO Screen => new()
        {
            ManagerId = 0,
            Layer = 3,
            Tag = ScreenTag.Default,
            Load = ScreenLoadCVO.Addressable("InfoScreen"),
            HasShowAnimation = false,
            HasHideAnimation = false,
        };

		private InfoScreenSignals _signals;
		private InfoScreenInternalSignals _internalSignals;

        public override void SignalBindings()
        {
            base.SignalBindings();
			_signals = InjectionBinderCrossContext.Bind<InfoScreenSignals>();
			_internalSignals = InjectionBinder.Bind<InfoScreenInternalSignals>();
        }

        public override void InjectionBindings()
        {
            base.InjectionBindings();
            InjectionBinder.Bind<IInfoScreenModel, InfoScreenModel>();
        }

        public override void CommandBindings()
        {
            base.CommandBindings();

            // Selecting a building or a unit opens the screen when it is closed and fills it with what was
            // selected; selecting another while it is open only refills it.
            CommandBinder.Bind(_signals.Incoming.ShowBuildingInfo)
                .ToSequence<OpenInfoScreenCommand>()
                .ToSequence<FillBuildingInfoCommand>();
            CommandBinder.Bind(_signals.Incoming.ShowUnitInfo)
                .ToSequence<OpenInfoScreenCommand>()
                .ToSequence<FillUnitInfoCommand>();

            // A cleared selection closes the screen only while it shows that kind - the building's selection is
            // cleared and a unit's made by the same press, in either order. Closing hands the unit cards back.
            CommandBinder.Bind(_signals.Incoming.HideBuildingInfo).ToSequence<HideInfoScreenCommand>(InfoSubjectType.Building);
            CommandBinder.Bind(_signals.Incoming.HideUnitInfo).ToSequence<HideInfoScreenCommand>(InfoSubjectType.Unit);
            CommandBinder.Bind(_internalSignals.InfoClosed).ToSequence<ReleaseUnitItemsCommand>();
        }
    }
}
