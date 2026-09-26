using FlowIoC.BaseModule.Injectable.Attributes;
using FlowIoC.BaseModule.ViewsMediators.Mediator;
using Modules.GameBoardModule.Data.ValueObjects;
using Modules.GameBoardModule.Signals;

namespace Modules.GameBoardModule.ViewsMediators
{
    public class GameBoardMediator : IMediator
    {
        [Inject]       private GameBoardView            _view            { get; set; }
        [InjectSignal] private GameBoardInternalSignals _internalSignals { get; set; }

        public void OnRegister()
        {
            _internalSignals.Draw.AddListener(OnDraw);
            _internalSignals.ShowBuilding.AddListener(OnShowBuilding);
            _internalSignals.ShowPlacementPreview.AddListener(OnShowPlacementPreview);
            _internalSignals.HidePlacementPreview.AddListener(_view.HidePlacementPreview);

            _view.PlacementConfirmClicked += OnPlacementConfirmClicked;
            _view.PlacementCancelClicked += OnPlacementCancelClicked;
        }

        public void OnRemove()
        {
            _internalSignals.Draw.RemoveListener(OnDraw);
            _internalSignals.ShowBuilding.RemoveListener(OnShowBuilding);
            _internalSignals.ShowPlacementPreview.RemoveListener(OnShowPlacementPreview);
            _internalSignals.HidePlacementPreview.RemoveListener(_view.HidePlacementPreview);

            _view.PlacementConfirmClicked -= OnPlacementConfirmClicked;
            _view.PlacementCancelClicked -= OnPlacementCancelClicked;
        }

        private void OnDraw(GameBoardLayoutVO layout) =>
            _view.Draw(layout.GridBounds, layout.FrameBounds, layout.CellSize, layout.Cells);

        private void OnShowBuilding(BoardBuildingVO building) =>
            _view.PlaceBuilding(building.Building, building.Type, building.Sprite, building.Area);

        private void OnShowPlacementPreview(PlacementPreviewVO preview) =>
            _view.ShowPlacementPreview(preview.Sprite, preview.Area, preview.PromptCentre);

        private void OnPlacementConfirmClicked() => _internalSignals.PlacementConfirmed.Dispatch();

        private void OnPlacementCancelClicked() => _internalSignals.PlacementCancelled.Dispatch();
    }
}
