using FlowIoC.BaseModule.Controller;
using FlowIoC.BaseModule.Injectable.Attributes;
using FlowIoC.PoolModule.Services;
using Modules.GameBoardModule.Data.ValueObjects;
using Modules.GameBoardModule.Entities;
using Modules.GameBoardModule.Models;
using Modules.GameBoardModule.Signals;
using Modules.GridModule.Services;
using UnityEngine;

namespace Modules.GameBoardModule.Controllers
{
    /// <summary>
    /// Lays the board out: the grid service builds an empty grid of the configured size, centred on the
    /// world origin, every cell gets a sprite out of the pool (group "board", warmed while the game
    /// loaded), and the frame is the grid plus its padding. The board view draws them, and the board's
    /// bounds - frame included - are announced. The board is built once, when the game starts.
    /// </summary>
    internal class BuildGameBoardCommand : Command
    {
        /// <summary>The key of the cell in CD_PoolGroup_Board.</summary>
        private const string CellPoolKey = "board_cell";

        [Inject]       private IGameBoardModel          _gameBoardModel  { get; set; }
        [Inject]       private IGridService             _gridService     { get; set; }
        [Inject]       private IPoolService             _poolService     { get; set; }
        [InjectSignal] private GameBoardSignals         _signals         { get; set; }
        [InjectSignal] private GameBoardInternalSignals _internalSignals { get; set; }

        public override void Execute()
        {
            float cellSize = _gameBoardModel.CellSize;
            Vector2Int gridSize = _gameBoardModel.GridSize;
            _gridService.Build(gridSize, cellSize);

            var tiles = new BoardCell[gridSize.x, gridSize.y];
            for (int column = 0; column < gridSize.x; column++)
            {
                for (int row = 0; row < gridSize.y; row++)
                    tiles[column, row] = _poolService.Get<BoardCell>(CellPoolKey, null);
            }

            Rect gridBounds = _gridService.Bounds;
            float padding = _gameBoardModel.FramePaddingInCells * cellSize;
            var frameBounds = new Rect(gridBounds.xMin - padding, gridBounds.yMin - padding,
                                       gridBounds.width + padding * 2f, gridBounds.height + padding * 2f);

            _internalSignals.Draw.Dispatch(new GameBoardLayoutVO(gridBounds, frameBounds, cellSize, _gridService.Cells, tiles));
            _signals.Outgoing.BoardBuilt.Dispatch(frameBounds);
        }
    }
}
