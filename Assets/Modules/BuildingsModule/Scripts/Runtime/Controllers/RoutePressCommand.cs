using FlowIoC.BaseModule.Controller;
using FlowIoC.BaseModule.Injectable.Attributes;
using Modules.BuildingsModule.Models;
using Modules.BuildingsModule.Signals;
using UnityEngine;

namespace Modules.BuildingsModule.Controllers
{
    /// <summary>
    /// Decides what a press on the board is for: while a building waits to be placed the press moves
    /// it, otherwise it picks what stands on the board.
    /// </summary>
    internal class RoutePressCommand : Command
    {
        [Inject]       private IPlacementModel          _placementModel  { get; set; }
        [InjectSignal] private BuildingsInternalSignals _internalSignals { get; set; }
        [SignalParam]  private Vector2                  _pointer         { get; set; }

        public override void Execute()
        {
            if (_placementModel.IsWaiting)
                _internalSignals.PlacementPressed.Dispatch(_pointer);
            else
                _internalSignals.BoardPressed.Dispatch(_pointer);
        }
    }
}
