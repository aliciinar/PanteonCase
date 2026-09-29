using FlowIoC.BaseModule.Controller;
using FlowIoC.BaseModule.Injectable.Attributes;
using Modules.GameBoardModule.Data.ValueObjects;
using Modules.GameBoardModule.Models;
using Modules.GameBoardModule.Signals;
using Modules.GridModule.Services;
using UnityEngine;

namespace Modules.GameBoardModule.Controllers
{
    /// <summary>
    /// Lays the board out: the grid service builds an empty grid of the configured size, centred on the
    /// world origin, and the frame is the grid plus its padding. The board view draws it - every cell one
    /// tile of CD_GameBoard's cell sprite - and the board's bounds, frame included, are announced. The
    /// board is built once, when the game starts.
    /// </summary>
    internal class BuildGameBoardCommand : Command
    {
        [Inject]       private IGameBoardModel          _gameBoardModel  { get; set; }
        [Inject]       private IGridService             _gridService     { get; set; }
        [InjectSignal] private GameBoardSignals         _signals         { get; set; }
        [InjectSignal] private GameBoardInternalSignals _internalSignals { get; set; }

        public override void Execute()
        {
            float cellSize = _gameBoardModel.CellSize;
            Vector2Int gridSize = _gameBoardModel.GridSize;
            _gridService.Build(gridSize, cellSize);

            Rect gridBounds = _gridService.Bounds;
            float padding = _gameBoardModel.FramePaddingInCells * cellSize;
            var frameBounds = new Rect(gridBounds.xMin - padding, gridBounds.yMin - padding,
                                       gridBounds.width + padding * 2f, gridBounds.height + padding * 2f);

            _internalSignals.Draw.Dispatch(new GameBoardLayoutVO(gridBounds, frameBounds, cellSize, _gridService.Cells,
                                                               _gameBoardModel.CellSprite));
            _signals.Outgoing.BoardBuilt.Dispatch(frameBounds);
        }
    }
}
