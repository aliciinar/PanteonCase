using FlowIoC.BaseModule.Function.ReturnableFunctions;
using FlowIoC.BaseModule.Injectable.Attributes;
using Modules.GameBoardModule.Models;
using UnityEngine;

namespace Modules.GameBoardModule.Controllers
{
    /// <summary>
    /// The nearest bottom-left cell that keeps an area of the given size inside the grid - so whatever
    /// the pointer does, a building being moved never hangs off the board.
    /// </summary>
    internal class ClampAreaToBoardFunction : FunctionReturn<Vector2Int, Vector2Int, Vector2Int>
    {
        [Inject] private IGameBoardModel _gameBoardModel { get; set; }

        public override Vector2Int Execute(Vector2Int origin, Vector2Int size)
        {
            Vector2Int gridSize = _gameBoardModel.GridSize;
            return new Vector2Int(Mathf.Clamp(origin.x, 0, Mathf.Max(0, gridSize.x - size.x)),
                                  Mathf.Clamp(origin.y, 0, Mathf.Max(0, gridSize.y - size.y)));
        }
    }
}
