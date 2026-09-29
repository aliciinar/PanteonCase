using FlowIoC.BaseModule.Contexts;
using FlowIoC.BaseModule.Controller.Commands;
using Modules.BuildingsModule.Controllers;
using Modules.BuildingsModule.Controllers.BoardBuildings;
using Modules.BuildingsModule.Models;
using Modules.BuildingsModule.Signals;
using Modules.BuildingsModule.ViewsMediators;

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
            InjectionBinder.Bind<IBuildingsModel, BuildingsModel>();
            InjectionBinder.Bind<IPlacementModel, PlacementModel>();
            InjectionBinder.Bind<IBuildingSelectionModel, BuildingSelectionModel>();
        }

        public override void MediationBindings()
        {
            base.MediationBindings();
            MediationBinder.Bind<PlacementPreviewView>().To<PlacementPreviewMediator>();
        }

        public override void CommandBindings()
        {
            base.CommandBindings();

            // A building picked in the production menu is previewed on the free area nearest the board's
            // centre - or at the centre, red, when none is left - and waits for the player's answer; a new
            // pick while one is waiting just moves the preview. From now on a press's drags are wanted.
            // SignalDispatchCommand releases no data, so it goes last.
            CommandBinder.Bind(_signals.Incoming.PlaceBuilding)
                .ToSequence<FindBuildingAreaCommand>()
                .ToSequence<PreviewPlacementCommand>()
                .ToSequence<SignalDispatchCommand>(_signals.Outgoing.PlacementStarted);

            // While a building waits to be placed, pressing the board moves it to the pressed cell, or grabs
            // it where it was pressed; while the press lasts it follows the pointer. Green where it fits, red
            // where not. With nothing waiting, a press is not the placement's and goes no further.
            CommandBinder.Bind(_signals.Incoming.PointerPressed)
                .ToSequence<GrabPlacementCommand>()
                .ToSequence<PreviewPlacementCommand>();

            CommandBinder.Bind(_signals.Incoming.PointerDragged)
                .ToSequence<DragPlacementCommand>()
                .ToSequence<PreviewPlacementCommand>();

            CommandBinder.Bind(_signals.Incoming.PointerReleased).ToSequence<ReleasePlacementCommand>();

            // The grid says what a press landed on - and says nothing while a building waits to be placed. A
            // building is selected - coloured on the board and announced, with its door and spawn point, so its
            // information shows; anything else clears the selection.
            CommandBinder.Bind(_signals.Incoming.SelectBuilding).ToSequence<SelectBuildingCommand>();
            CommandBinder.Bind(_signals.Incoming.ClearSelection).ToSequence<ClearBuildingSelectionCommand>();

            // Green tick: the building is put on the board - its cells hold its data, and its data its object,
            // from now on. The placement travels from step to step, so the preview is hidden last -
            // SignalDispatchCommand releases no data - and the placement is over: no drags are wanted.
            CommandBinder.Bind(_internalSignals.PlacementConfirmed)
                .ToSequence<TakePendingPlacementCommand>()
                .ToSequence<PlaceBoardBuildingCommand>()
                .ToSequence<SignalDispatchCommand>(_internalSignals.HidePlacementPreview)
                .ToSequence<SignalDispatchCommand>(_signals.Outgoing.PlacementEnded);

            // A struck building shows the hit; a destroyed one - already off the board - has its object put back
            // in the pool.
            CommandBinder.Bind(_signals.Incoming.BuildingDamaged).ToSequence<ShowBoardBuildingHitCommand>();
            CommandBinder.Bind(_signals.Incoming.RemoveBuilding).ToSequence<RemoveBoardBuildingCommand>();

            // Red cross: nothing is placed, the preview goes away and the placement is over.
            CommandBinder.Bind(_internalSignals.PlacementCancelled)
                .ToSequence<DiscardPendingPlacementCommand>()
                .ToSequence<SignalDispatchCommand>(_internalSignals.HidePlacementPreview)
                .ToSequence<SignalDispatchCommand>(_signals.Outgoing.PlacementEnded);
        }
    }
}
