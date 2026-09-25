using FlowIoC.BaseModule.Controller;
using FlowIoC.BaseModule.Injectable.Attributes;
using Modules.GameBoardModule.Data.ValueObjects;
using Modules.GameBoardModule.Models;
using Modules.GameBoardModule.Signals;

namespace Modules.GameBoardModule.Controllers
{
    /// <summary>Hands the model's layout to the board view and announces the board's bounds.</summary>
    internal class BuildGameBoardCommand : Command
    {
        [Inject]       private IGameBoardModel          _gameBoardModel  { get; set; }
        [InjectSignal] private GameBoardSignals         _signals         { get; set; }
        [InjectSignal] private GameBoardInternalSignals _internalSignals { get; set; }

        public override void Execute()
        {
            _internalSignals.Draw.Dispatch(new GameBoardLayoutVO(_gameBoardModel.GridBounds,
                                                                 _gameBoardModel.FrameBounds,
                                                                 _gameBoardModel.CellSize,
                                                                 _gameBoardModel.Cells));

            _signals.Outgoing.BoardBuilt.Dispatch(_gameBoardModel.FrameBounds);
        }
    }
}
