using FlowIoC.BaseModule.Controller;
using FlowIoC.BaseModule.Injectable.Attributes;
using FlowIoC.BaseModule.Provider.Update;
using Modules.InputModule.Signals;

namespace Modules.InputModule.Controllers
{
    /// <summary>
    /// A press started: from this frame until it ends, IUpdateProvider dispatches PollPointer every
    /// frame and the view reads the pointer. Between presses nothing runs at all.
    /// </summary>
    internal class StartPointerPollingCommand : Command
    {
        [Inject]       private IUpdateProvider      _updateProvider  { get; set; }
        [InjectSignal] private InputInternalSignals _internalSignals { get; set; }

        public override void Execute()
        {
            // Removed first, so a press whose release was never seen - the window lost focus
            // mid-press - does not leave the pointer polled twice a frame.
            _updateProvider.RemoveUpdate(_internalSignals.PollPointer.Dispatch);
            _updateProvider.AddUpdate(_internalSignals.PollPointer.Dispatch);
        }
    }
}
