using FlowIoC.BaseModule.Controller;
using FlowIoC.BaseModule.Injectable.Attributes;
using Modules.BuildingsModule.Models;
using Modules.BuildingsModule.Signals;

namespace Modules.BuildingsModule.Controllers
{
    /// <summary>
    /// Something other than a building was pressed - a unit, a free cell, off the grid: no building is
    /// selected any more. While a building waits to be placed a press only moves that building, so the
    /// selection stays.
    /// </summary>
    internal class ClearBuildingSelectionCommand : Command
    {
        [Inject]       private IPlacementModel  _placementModel { get; set; }
        [InjectSignal] private BuildingsSignals _signals        { get; set; }

        public override void Execute()
        {
            if (_placementModel.IsWaiting) return;

            _signals.Outgoing.SelectionCleared.Dispatch();
        }
    }
}
