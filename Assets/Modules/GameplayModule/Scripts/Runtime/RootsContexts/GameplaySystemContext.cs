using FlowIoC.BaseModule.Contexts;
using Modules.GameplayModule.Controllers;
using Modules.GameplayModule.Models;
using Modules.GameplayModule.Signals;

namespace Modules.GameplayModule.RootsContexts
{
    public class GameplaySystemContext : Context
    {
        private GameplaySignals _signals;

        public override void SignalBindings()
        {
            base.SignalBindings();
            _signals = InjectionBinderCrossContext.Bind<GameplaySignals>();
        }

        public override void InjectionBindings()
        {
            base.InjectionBindings();
            InjectionBinder.Bind<IGameStatusModel, GameStatusModel>();
        }

        public override void CommandBindings()
        {
            base.CommandBindings();

            // The game is played one action at a time: while one runs, RD_GameStatus is locked and no order is taken.
            CommandBinder.Bind(_signals.Incoming.Lock).ToSequence<SetGameLockCommand>(true);
            CommandBinder.Bind(_signals.Incoming.Unlock).ToSequence<SetGameLockCommand>(false);
        }
    }
}
