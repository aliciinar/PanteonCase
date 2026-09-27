using FlowIoC.BaseModule.Connectors;
using FlowIoC.BaseModule.Contexts;
using Modules.GameplayModule.Signals;
using Modules.UnitsModule.Signals;

namespace Modules.ConnectorModule.RootsContexts
{
    /// <summary>What the gameplay module is told: an action started, so the game locks, and it ended, so it unlocks.</summary>
    public class GameplayConnectorSubContext : Context
    {
        private GameplaySignals _gameplaySignals;
        private UnitsSignals _unitsSignals;

        public override void Setup()
        {
            base.Setup();

            _gameplaySignals = InjectionBinderCrossContext.GetInstance<GameplaySignals>();
            _unitsSignals = InjectionBinderCrossContext.GetInstance<UnitsSignals>();

            IncomingSignals();
        }

        private void IncomingSignals()
        {
            _unitsSignals.Outgoing.ActionStarted.Connect(_gameplaySignals.Incoming.Lock);
            _unitsSignals.Outgoing.ActionEnded.Connect(_gameplaySignals.Incoming.Unlock);
        }

        public override void DestroyContext()
        {
            UnbindIncomingSignals();

            base.DestroyContext();
        }

        private void UnbindIncomingSignals()
        {
            _unitsSignals.Outgoing.ActionStarted.Disconnect();
            _unitsSignals.Outgoing.ActionEnded.Disconnect();
        }
    }
}
