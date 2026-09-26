using FlowIoC.BaseModule.Controller;
using FlowIoC.BaseModule.Injectable.Attributes;
using Modules.BuildingsModule.Data.ValueObjects;
using Modules.BuildingsModule.Models;

namespace Modules.BuildingsModule.Controllers
{
    /// <summary>
    /// Takes the placement that was waiting for the player and hands it to the next step; nothing is
    /// waiting afterwards.
    /// </summary>
    internal class TakePendingPlacementCommand : Command
    {
        [Inject] private IPlacementModel _placementModel { get; set; }

        public override void Execute()
        {
            Retain();

            BuildingPlacementVO placement = _placementModel.PendingPlacement;
            _placementModel.PendingPlacement = null;
            _placementModel.IsDragging = false;

            Release(placement);
        }
    }
}
