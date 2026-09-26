using FlowIoC.BaseModule.Controller;
using FlowIoC.BaseModule.Injectable.Attributes;
using Modules.GameBoardModule.Models;

namespace Modules.GameBoardModule.Controllers
{
    /// <summary>Drops the placement that was waiting for the player; nothing is placed.</summary>
    internal class DiscardPendingPlacementCommand : Command
    {
        [Inject] private IGameBoardModel _gameBoardModel { get; set; }

        public override void Execute() => _gameBoardModel.PendingPlacement = null;
    }
}
