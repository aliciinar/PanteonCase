using FlowIoC.BaseModule.Contexts;
using FlowIoC.BaseModule.Controller.Commands;
using Modules.GameBoardModule.Controllers;
using Modules.GameBoardModule.Models;
using Modules.GameBoardModule.Signals;
using Modules.GameBoardModule.ViewsMediators;

namespace Modules.GameBoardModule.RootsContexts
{

    public class GameBoardSystemContext : Context
    {
		private GameBoardSignals _signals;

		private GameBoardInternalSignals _internalSignals;

        public override void SignalBindings()
        {
            base.SignalBindings();
			_internalSignals = InjectionBinder.Bind<GameBoardInternalSignals>();
			_signals = InjectionBinderCrossContext.Bind<GameBoardSignals>();
        }

        public override void InjectionBindings()
        {
            base.InjectionBindings();
            InjectionBinder.Bind<IGameBoardModel, GameBoardModel>();
        }

        public override void MediationBindings()
        {
            base.MediationBindings();
            MediationBinder.Bind<GameBoardView>().To<GameBoardMediator>();
        }

        public override void CommandBindings()
        {
            base.CommandBindings();
            CommandBinder.Bind(_signals.Incoming.BuildBoard).ToSequence<BuildGameBoardCommand>();
            CommandBinder.Bind(_signals.Incoming.FindFreeArea).ToSequence<FindFreeAreaCommand>();

            // A building picked in the production menu is previewed on the free area nearest the board's
            // centre and waits for the player's answer; a new pick while one is waiting just moves the preview.
            CommandBinder.Bind(_signals.Incoming.PlaceBuilding)
                .ToSequence<FindBuildingAreaCommand>()
                .ToSequence<PreviewPlacementCommand>();

            // Green tick: the building takes its cells and is built there. The placement travels from
            // step to step, so the preview is hidden last - SignalDispatchCommand releases no data.
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
