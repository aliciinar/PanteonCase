using FlowIoC.ScreenModule.Data;
using FlowIoC.ScreenModule.Enums;
using FlowIoC.ScreenModule.RootsContexts;
using Modules.BuildingsModule.ProductionMenuScreenModule.Controllers;
using Modules.BuildingsModule.ProductionMenuScreenModule.Models;
using Modules.BuildingsModule.ProductionMenuScreenModule.Signals;
using Modules.BuildingsModule.ProductionMenuScreenModule.ViewsMediators;

namespace Modules.BuildingsModule.ProductionMenuScreenModule.RootsContexts
{
    public class ProductionMenuScreenContext : ScreenSubContext<ProductionMenuScreenView, ProductionMenuScreenMediator>
    {
        protected override ScreenCVO Screen => new()
        {
            ManagerId = 0,
            Layer = 1,
            Tag = ScreenTag.Default,
            Load = ScreenLoadCVO.Addressable("ProductionMenuScreen"),
            HasShowAnimation = false,
            HasHideAnimation = false,
        };

        private ProductionMenuScreenSignals _signals;
        private ProductionMenuScreenInternalSignals _internalSignals;

        public override void SignalBindings()
        {
            base.SignalBindings();
            _signals = InjectionBinderCrossContext.Bind<ProductionMenuScreenSignals>();
            _internalSignals = InjectionBinder.Bind<ProductionMenuScreenInternalSignals>();
        }

        public override void InjectionBindings()
        {
            base.InjectionBindings();
            InjectionBinder.Bind<IProductionMenuModel, ProductionMenuModel>();
        }

        public override void CommandBindings()
        {
            base.CommandBindings();

            // Opening lays the menu out against the screen; so does a resize, once the UI has rescaled.
            CommandBinder.Bind(_signals.Incoming.OpenProductionMenuScreen)
                .ToSequence<OpenProductionMenuScreenCommand>()
                .ToSequence<RelayoutProductionMenuCommand>();

            CommandBinder.Bind(_signals.Incoming.ScreenResized)
                .ToSequence<WaitForLayoutCommand>()
                .ToSequence<RelayoutProductionMenuCommand>();

            // Every scroll works out the visible rows; only when they changed are the cards of the rows that left
            // moved to the rows that came in. The pool is asked only when the number of cards has to change.
            CommandBinder.Bind(_internalSignals.Scrolled)
                .ToSequence<UpdateVisibleRowsCommand>()
                .ToSequence<RecycleProductionRowsCommand>();

            CommandBinder.Bind(_internalSignals.MenuClosed).ToSequence<CloseProductionMenuCommand>();
        }
    }
}
