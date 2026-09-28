using FlowIoC.ScreenModule.Data;
using FlowIoC.ScreenModule.Enums;
using FlowIoC.ScreenModule.RootsContexts;
using Modules.ScreenModule.PopupScreenModule.Controllers;
using Modules.ScreenModule.PopupScreenModule.Models;
using Modules.ScreenModule.PopupScreenModule.Signals;
using Modules.ScreenModule.PopupScreenModule.ViewsMediators;

namespace Modules.ScreenModule.PopupScreenModule.RootsContexts
{
    public class PopupScreenContext : ScreenSubContext<PopupScreenView, PopupScreenMediator>
    {
        // Above the HUD (1 - 3), below the loading screens (8, 9).
        protected override ScreenCVO Screen => new()
        {
            ManagerId = 0,
            Layer = 7,
            Tag = ScreenTag.Default,
            Load = ScreenLoadCVO.Addressable("PopupScreen"),
            HasShowAnimation = true,
            HasHideAnimation = false,
        };

        private PopupScreenSignals _signals;

        public override void SignalBindings()
        {
            base.SignalBindings();
            _signals = InjectionBinderCrossContext.Bind<PopupScreenSignals>();
        }

        public override void InjectionBindings()
        {
            base.InjectionBindings();
            InjectionBinder.Bind<IPopupScreenModel, PopupScreenModel>();
        }

        public override void CommandBindings()
        {
            base.CommandBindings();

            // Any module's message opens the popup, or replaces the one it shows. It closes itself (Mediator).
            CommandBinder.Bind(_signals.Incoming.ShowPopup).ToSequence<ShowPopupCommand>();
        }
    }
}
