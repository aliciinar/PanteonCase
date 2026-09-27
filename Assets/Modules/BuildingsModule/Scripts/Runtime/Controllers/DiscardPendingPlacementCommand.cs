using FlowIoC.BaseModule.Controller;
using FlowIoC.BaseModule.Injectable.Attributes;
using Modules.BuildingsModule.Models;

namespace Modules.BuildingsModule.Controllers
{
    /// <summary>Drops the placement that was waiting for the player; nothing is placed.</summary>
    internal class DiscardPendingPlacementCommand : Command
    {
        [Inject] private IPlacementModel _placementModel { get; set; }

        public override void Execute() => _placementModel.Discard();
    }
}
