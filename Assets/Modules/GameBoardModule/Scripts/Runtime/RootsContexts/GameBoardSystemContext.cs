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
            CommandBinder.Bind(_internalSignals.Build).ToSequence<BuildGameBoardCommand>();
        }

        public override void Setup()
        {
            base.Setup();
        }

        public override void Launch()
        {
            base.Launch();
            _internalSignals.Build.Dispatch();
        }
    }
}
