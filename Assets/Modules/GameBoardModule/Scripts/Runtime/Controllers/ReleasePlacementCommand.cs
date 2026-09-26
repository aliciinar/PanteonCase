using FlowIoC.BaseModule.Controller;
using FlowIoC.BaseModule.Injectable.Attributes;
using Modules.GameBoardModule.Models;

namespace Modules.GameBoardModule.Controllers
{
    /// <summary>The press ended: a grabbed placement stays where it is and stops following the pointer.</summary>
    internal class ReleasePlacementCommand : Command
    {
        [Inject] private IGameBoardModel _gameBoardModel { get; set; }

        public override void Execute() => _gameBoardModel.IsDraggingPlacement = false;
    }
}
