using FlowIoC.BaseModule.Contexts;
using FlowIoC.BaseModule.Controller.Commands;
using Modules.InputModule.Controllers;
using Modules.InputModule.Models;
using Modules.InputModule.Signals;
using Modules.InputModule.ViewsMediators;

namespace Modules.InputModule.RootsContexts
{
    public class InputSystemContext : Context
    {
        private InputSignals _signals;
        private InputInternalSignals _internalSignals;

        public override void SignalBindings()
        {
            base.SignalBindings();
            _internalSignals = InjectionBinder.Bind<InputInternalSignals>();
            _signals = InjectionBinderCrossContext.Bind<InputSignals>();
        }

        public override void InjectionBindings()
        {
            base.InjectionBindings();
            InjectionBinder.Bind<IPointerModel, PointerModel>();
        }

        public override void MediationBindings()
        {
            base.MediationBindings();
            MediationBinder.Bind<InputView>().To<InputMediator>();
        }

        public override void CommandBindings()
        {
            base.CommandBindings();

            // The pointer is read every frame only while a press lasts, through IUpdateProvider.
            CommandBinder.Bind(_internalSignals.PressStarted).ToSequence<StartPointerPollingCommand>();

            // A press is announced from start to end only when it began on the world, not over UI.
            CommandBinder.Bind(_internalSignals.PointerPressed).ToSequence<BeginPressCommand>();
            CommandBinder.Bind(_internalSignals.PointerDragged).ToSequence<ContinuePressCommand>();
            CommandBinder.Bind(_internalSignals.PointerReleased)
                .ToSequence<EndPressCommand>()
                .ToSequence<StopPointerPollingCommand>();

            // The pointer's moves are read only while someone needs them - a building being placed - from
            // StartListeningToDrag to StopListeningToDrag. Presses and releases are always heard.
            CommandBinder.Bind(_signals.Incoming.StartListeningToDrag).ToSequence<SignalDispatchCommand<bool>>(_internalSignals.DragListening, true);
            CommandBinder.Bind(_signals.Incoming.StopListeningToDrag).ToSequence<SignalDispatchCommand<bool>>(_internalSignals.DragListening, false);
        }
    }
}
