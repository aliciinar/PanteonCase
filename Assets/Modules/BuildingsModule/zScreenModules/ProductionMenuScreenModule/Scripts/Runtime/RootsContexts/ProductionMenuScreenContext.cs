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

            // Every scroll decides which rows are on screen; rows that come into view take cards from
            // the pool, and the cards of rows that left go back to it.
            CommandBinder.Bind(_internalSignals.Scrolled).ToSequence<UpdateVisibleRowsCommand>();
            CommandBinder.Bind(_internalSignals.RowsEntered).ToSequence<SpawnProductionRowsCommand>();
            CommandBinder.Bind(_internalSignals.RowsLeft).ToSequence<ReleaseProductionRowsCommand>();
            CommandBinder.Bind(_internalSignals.MenuClosed).ToSequence<CloseProductionMenuCommand>();
        }
    }
}
