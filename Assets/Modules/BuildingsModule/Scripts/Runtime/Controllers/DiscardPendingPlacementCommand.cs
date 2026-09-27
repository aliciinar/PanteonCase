using FlowIoC.BaseModule.Controller;
using FlowIoC.BaseModule.Injectable.Attributes;
using Modules.BuildingsModule.Models;

namespace Modules.BuildingsModule.Controllers
{
    /// <summary>
    /// Drops the placement that was waiting for the player; nothing is placed. While an action runs (RD_GameStatus)
    /// the cross is not taken and the placement waits on.
    /// </summary>
    internal class DiscardPendingPlacementCommand : Command
    {
        [Inject] private IPlacementModel _placementModel { get; set; }

        public override void Execute()
        {
            Retain();

            if (_placementModel.IsGameLocked)
            {
                Stop();
                return;
            }

            _placementModel.Discard();
            Release();
        }
    }
}
