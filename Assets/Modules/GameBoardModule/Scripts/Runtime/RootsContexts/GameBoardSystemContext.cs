using FlowIoC.BaseModule.Contexts;
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

            // A building picked in the production menu is shown on the free area nearest the board's centre.
            CommandBinder.Bind(_signals.Incoming.PlaceBuilding)
                .ToSequence<FindBuildingAreaCommand>()
                .ToSequence<ShowBuildingCommand>();
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
