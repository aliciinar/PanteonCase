using FlowIoC.BaseModule.Function.ReturnableFunctions;
using FlowIoC.BaseModule.Injectable.Attributes;
using Modules.GameBoardModule.Models;
using Modules.GameBoardModule.Shared.Data.ValueObjects;
using UnityEngine;

namespace Modules.GameBoardModule.Controllers
{
    /// <summary>
    /// Whether an area of the board is free: it lies entirely inside the grid and nothing stands on any
    /// of its cells. The area is given by its bottom-left cell and its size in cells. A Function rather
    /// than a Command because it answers with a value, and every command that places something on the
    /// board - searching for room, following the mouse - asks the same question.
    /// </summary>
    internal class IsAreaFreeFunction : FunctionReturn<bool, Vector2Int, Vector2Int>
    {
        [Inject] private IGameBoardModel _gameBoardModel { get; set; }

        public override bool Execute(Vector2Int origin, Vector2Int size)
        {
            Vector2Int gridSize = _gameBoardModel.GridSize;

            if (origin.x < 0 || origin.y < 0 || origin.x + size.x > gridSize.x || origin.y + size.y > gridSize.y)
                return false;

            CellVO[,] cells = _gameBoardModel.Cells;

            for (int column = origin.x; column < origin.x + size.x; column++)
            {
                for (int row = origin.y; row < origin.y + size.y; row++)
                {
                    if (!cells[column, row].IsFree) return false;
                }
            }

            return true;
        }
    }
}
