using FlowIoC.BaseModule.Controller;
using FlowIoC.BaseModule.Injectable.Attributes;
using FlowIoC.BaseModule.Provider.Update;
using Modules.InputModule.Signals;

namespace Modules.InputModule.Controllers
{
    /// <summary>
    /// A button went down: from this frame until both are up, IUpdateProvider dispatches PollPointer
    /// every frame and the view reads the pointer. Between presses nothing runs at all.
    /// </summary>
    internal class StartPointerPollingCommand : Command
    {
        [Inject]       private IUpdateProvider      _updateProvider  { get; set; }
        [InjectSignal] private InputInternalSignals _internalSignals { get; set; }

        public override void Execute()
        {
            // Removed first, so the second button going down while the first is held - or a press whose
            // release was never seen - does not leave the pointer polled twice a frame.
            _updateProvider.RemoveUpdate(_internalSignals.PollPointer.Dispatch);
            _updateProvider.AddUpdate(_internalSignals.PollPointer.Dispatch);
        }
    }
}
