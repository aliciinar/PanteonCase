using FlowIoC.BaseModule.Controller;
using FlowIoC.BaseModule.Injectable.Attributes;
using FlowIoC.BaseModule.Provider.Update;
using Modules.InputModule.Signals;

namespace Modules.InputModule.Controllers
{
    /// <summary>
    /// The press ended: the pointer is no longer polled. The delegate removed is equal to the one
    /// added - the same signal's Dispatch - so nothing has to be kept between the two commands.
    /// </summary>
    internal class StopPointerPollingCommand : Command
    {
        [Inject]       private IUpdateProvider      _updateProvider  { get; set; }
        [InjectSignal] private InputInternalSignals _internalSignals { get; set; }

        public override void Execute() => _updateProvider.RemoveUpdate(_internalSignals.PollPointer.Dispatch);
    }
}
