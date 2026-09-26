using FlowIoC.BaseModule.Controller;
using FlowIoC.BaseModule.Injectable.Attributes;
using Modules.GameBoardModule.Data.ValueObjects;
using Modules.GameBoardModule.Models;

namespace Modules.GameBoardModule.Controllers
{
    /// <summary>
    /// Takes the placement that was waiting for the player and hands it to the next step; nothing is
    /// waiting afterwards.
    /// </summary>
    internal class TakePendingPlacementCommand : Command
    {
        [Inject] private IGameBoardModel _gameBoardModel { get; set; }

        public override void Execute()
        {
            Retain();

            BuildingPlacementVO placement = _gameBoardModel.PendingPlacement;
            _gameBoardModel.PendingPlacement = null;

            Release(placement);
        }
    }
}
