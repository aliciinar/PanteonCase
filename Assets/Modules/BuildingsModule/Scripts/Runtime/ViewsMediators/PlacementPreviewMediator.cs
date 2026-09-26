using FlowIoC.BaseModule.Injectable.Attributes;
using FlowIoC.BaseModule.ViewsMediators.Mediator;
using Modules.BuildingsModule.Data.ValueObjects;
using Modules.BuildingsModule.Signals;

namespace Modules.BuildingsModule.ViewsMediators
{
    public class PlacementPreviewMediator : IMediator
    {
        [Inject]       private PlacementPreviewView     _view            { get; set; }
        [InjectSignal] private BuildingsInternalSignals _internalSignals { get; set; }

        public void OnRegister()
        {
            _internalSignals.ShowPlacementPreview.AddListener(OnShowPlacementPreview);
            _internalSignals.HidePlacementPreview.AddListener(_view.Hide);

            _view.ConfirmClicked += OnConfirmClicked;
            _view.CancelClicked += OnCancelClicked;
        }

        public void OnRemove()
        {
            _internalSignals.ShowPlacementPreview.RemoveListener(OnShowPlacementPreview);
            _internalSignals.HidePlacementPreview.RemoveListener(_view.Hide);

            _view.ConfirmClicked -= OnConfirmClicked;
            _view.CancelClicked -= OnCancelClicked;
        }

        private void OnShowPlacementPreview(PlacementPreviewVO preview) =>
            _view.Show(preview.Sprite, preview.Area, preview.PromptCentre, preview.Fits, preview.CellSize);

        private void OnConfirmClicked() => _internalSignals.PlacementConfirmed.Dispatch();

        private void OnCancelClicked() => _internalSignals.PlacementCancelled.Dispatch();
    }
}
