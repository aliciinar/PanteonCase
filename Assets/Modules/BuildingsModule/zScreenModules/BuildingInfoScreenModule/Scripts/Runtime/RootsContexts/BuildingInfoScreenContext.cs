using FlowIoC.ScreenModule.Data;
using FlowIoC.ScreenModule.Enums;
using FlowIoC.ScreenModule.RootsContexts;
using Modules.BuildingsModule.BuildingInfoScreenModule.Controllers;
using Modules.BuildingsModule.BuildingInfoScreenModule.Models;
using Modules.BuildingsModule.BuildingInfoScreenModule.Signals;
using Modules.BuildingsModule.BuildingInfoScreenModule.ViewsMediators;

namespace Modules.BuildingsModule.BuildingInfoScreenModule.RootsContexts
{
    public class BuildingInfoScreenContext : ScreenSubContext<BuildingInfoScreenView, BuildingInfoScreenMediator>
    {
        // One layer above the information panel (2), inside which it is drawn.
        protected override ScreenCVO Screen => new()
        {
            ManagerId = 0,
            Layer = 3,
            Tag = ScreenTag.Default,
            Load = ScreenLoadCVO.Addressable("BuildingInfoScreen"),
            HasShowAnimation = false,
            HasHideAnimation = false,
        };

		private BuildingInfoScreenSignals _signals;
		private BuildingInfoScreenInternalSignals _internalSignals;

        public override void SignalBindings()
        {
            base.SignalBindings();
			_signals = InjectionBinderCrossContext.Bind<BuildingInfoScreenSignals>();
			_internalSignals = InjectionBinder.Bind<BuildingInfoScreenInternalSignals>();
        }

        public override void InjectionBindings()
        {
            base.InjectionBindings();
            InjectionBinder.Bind<IBuildingInfoModel, BuildingInfoModel>();
        }

        public override void CommandBindings()
        {
            base.CommandBindings();

            // Selecting a building opens the screen when it is closed and fills it with that building;
            // selecting another while it is open only refills it.
            CommandBinder.Bind(_signals.Incoming.ShowBuildingInfo)
                .ToSequence<OpenBuildingInfoScreenCommand>()
                .ToSequence<FillBuildingInfoCommand>();

            // Closing it hands its unit cards back to the pool.
            CommandBinder.Bind(_signals.Incoming.HideBuildingInfo).ToSequence<HideBuildingInfoScreenCommand>();
            CommandBinder.Bind(_internalSignals.InfoClosed).ToSequence<ReleaseUnitItemsCommand>();
        }
    }
}
