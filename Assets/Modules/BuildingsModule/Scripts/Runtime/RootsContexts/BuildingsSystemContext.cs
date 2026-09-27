using FlowIoC.BaseModule.Contexts;
using FlowIoC.BaseModule.Controller.Commands;
using Modules.BuildingsModule.Controllers;
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
            InjectionBinder.Bind<IBoardBuildingsModel, BoardBuildingsModel>();
        }

        public override void MediationBindings()
        {
            base.MediationBindings();
            MediationBinder.Bind<PlacedBuildingsView>().To<PlacedBuildingsMediator>();
            MediationBinder.Bind<PlacementPreviewView>().To<PlacementPreviewMediator>();
        }

        public override void CommandBindings()
        {
            base.CommandBindings();

            // A building picked in the production menu is previewed on the free area nearest the board's
            // centre - or at the centre, red, when none is left - and waits for the player's answer; a new
            // pick while one is waiting just moves the preview.
            CommandBinder.Bind(_signals.Incoming.PlaceBuilding)
                .ToSequence<FindBuildingAreaCommand>()
                .ToSequence<PreviewPlacementCommand>();

            // A press on the board is for the building waiting to be placed while one waits, and picks
            // what stands on the board otherwise.
            CommandBinder.Bind(_signals.Incoming.PointerPressed).ToSequence<RoutePressCommand>();

            // Pressing the board moves the waiting building to the pressed cell, or grabs it where it was
            // pressed; while the press lasts it follows the pointer. Green where it fits, red where not.
            CommandBinder.Bind(_internalSignals.PlacementPressed)
                .ToSequence<GrabPlacementCommand>()
                .ToSequence<PreviewPlacementCommand>();

            CommandBinder.Bind(_signals.Incoming.PointerDragged)
                .ToSequence<DragPlacementCommand>()
                .ToSequence<PreviewPlacementCommand>();

            CommandBinder.Bind(_signals.Incoming.PointerReleased).ToSequence<ReleasePlacementCommand>();

            // With nothing waiting, a press selects the building under it - announced, so its information
            // shows - or clears the selection when it lands anywhere else.
            CommandBinder.Bind(_internalSignals.BoardPressed).ToSequence<SelectBuildingAtCommand>();

            // A unit picked in the information panel comes out of the selected building.
            CommandBinder.Bind(_signals.Incoming.ProduceUnit).ToSequence<ProduceUnitCommand>();

            // Green tick: the building takes its cells, is recorded under the grid's id for them and is
            // built there. The placement travels from step to step, so the preview is hidden last -
            // SignalDispatchCommand releases no data.
            CommandBinder.Bind(_internalSignals.PlacementConfirmed)
                .ToSequence<TakePendingPlacementCommand>()
                .ToSequence<OccupyBuildingAreaCommand>()
                .ToSequence<ShowBuildingCommand>()
                .ToSequence<SignalDispatchCommand>(_internalSignals.HidePlacementPreview);

            // Red cross: nothing is placed and the preview goes away.
            CommandBinder.Bind(_internalSignals.PlacementCancelled)
                .ToSequence<DiscardPendingPlacementCommand>()
                .ToSequence<SignalDispatchCommand>(_internalSignals.HidePlacementPreview);
        }

        public override void Setup()
        {
            base.Setup();
        }

        public override void Launch()
        {
            base.Launch();
        }
    }
}
