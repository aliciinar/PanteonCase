using System.Collections.Generic;
using FlowIoC.BaseModule.Injectable.Attributes;
using FlowIoC.BaseModule.ViewsMediators.Mediator;
using FlowIoC.ScreenModule.Enums;
using FlowIoC.ScreenModule.ViewsMediators.Screen;
using Modules.BuildingsModule.ProductionMenuScreenModule.Entities;
using Modules.BuildingsModule.ProductionMenuScreenModule.Signals;
using Modules.BuildingsModule.Shared.Enums;

namespace Modules.BuildingsModule.ProductionMenuScreenModule.ViewsMediators
{
    /// <summary>
    /// Turns what the player does on the menu into signals: a scroll, a card click, and the screen
    /// closing. Everything the menu shows is put there by the commands, which reach the open screen
    /// through the screen service.
    /// </summary>
    public class ProductionMenuScreenMediator : IMediator
    {
        [Inject]       private ProductionMenuScreenView            _view            { get; set; }
        [InjectSignal] private ProductionMenuScreenSignals         _signals         { get; set; }
        [InjectSignal] private ProductionMenuScreenInternalSignals _internalSignals { get; set; }

        public void OnRegister()
        {
            _view.ShowCompleted += OnScreenShown;
            _view.HideCompleted += OnScreenHidden;
        }

        public void OnRemove()
        {
            _view.ShowCompleted -= OnScreenShown;
            _view.HideCompleted -= OnScreenHidden;
            Unsubscribe();
        }

        private void OnScreenShown(IScreenBody screen)
        {
            _view.Scrolled += OnScrolled;
            _view.ItemClicked += OnItemClicked;
        }

        private void OnScreenHidden(IScreenBody screen)
        {
            Unsubscribe();

            List<ProductionItem> cards = _view.RemoveAllRows();
            _internalSignals.MenuClosed.Dispatch(cards);
        }

        private void Unsubscribe()
        {
            _view.Scrolled -= OnScrolled;
            _view.ItemClicked -= OnItemClicked;
        }

        private void OnScrolled() => _internalSignals.Scrolled.Dispatch();

        private void OnItemClicked(BuildType buildType)
        {
            if (_view.Data.State != ScreenState.AvailableToSendSignal) return;

            _signals.Outgoing.BuildTypeSelected.Dispatch(buildType);
        }
    }
}
