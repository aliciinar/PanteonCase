using System.Collections.Generic;
using FlowIoC.BaseModule.Function.Provider;
using FlowIoC.BaseModule.Function.ReturnableFunctions;
using FlowIoC.BaseModule.Injectable.Attributes;
using Modules.GameBoardModule.Models;
using UnityEngine;

namespace Modules.GameBoardModule.Controllers
{
    /// <summary>
    /// Where an area of the given size fits on the board, as close to the board's centre as possible:
    /// the bottom-left cell of the nearest fit, or null when it fits nowhere. A Function because it
    /// answers with a value, and several flows ask it - finding room for a building, and whoever
    /// dispatches FindFreeArea.
    ///
    /// Breadth-first search over the grid. Each cell is tried as the area's bottom-left corner, starting
    /// from the corner that would centre the area on the board, then its four neighbours, then theirs -
    /// so corners are tried in order of how many steps they are from the centred position, and the
    /// first one where the whole area is free is the nearest fit. If the search runs out of cells,
    /// nothing fits anywhere.
    /// </summary>
    internal class FindFreeAreaFunction : FunctionReturn<Vector2Int?, Vector2Int>
    {
        private static readonly Vector2Int[] NeighbourSteps =
        {
            Vector2Int.up, Vector2Int.right, Vector2Int.down, Vector2Int.left
        };

        [Inject] private IGameBoardModel   _gameBoardModel   { get; set; }
        [Inject] private IFunctionProvider _functionProvider { get; set; }

        public override Vector2Int? Execute(Vector2Int size)
        {
            Vector2Int gridSize = _gameBoardModel.GridSize;

            // The corner that centres the area on the board. An area larger than the board starts at
            // 0 and is rejected everywhere, which ends in null.
            var start = new Vector2Int(Mathf.Max(0, (gridSize.x - size.x) / 2),
                                       Mathf.Max(0, (gridSize.y - size.y) / 2));

            var frontier = new Queue<Vector2Int>();
            var visited = new HashSet<Vector2Int>();
            frontier.Enqueue(start);
            visited.Add(start);

            while (frontier.Count > 0)
            {
                Vector2Int cell = frontier.Dequeue();

                if (IsAreaFree(cell, size)) return cell;

                foreach (Vector2Int step in NeighbourSteps)
                {
                    Vector2Int next = cell + step;
                    bool isOnGrid = next.x >= 0 && next.y >= 0 && next.x < gridSize.x && next.y < gridSize.y;

                    if (isOnGrid && visited.Add(next)) frontier.Enqueue(next);
                }
            }

            return null;
        }

        private bool IsAreaFree(Vector2Int origin, Vector2Int size) =>
            _functionProvider.Call<IsAreaFreeFunction>().AddParams(origin, size).ExecuteAndGetResult<bool>();
    }
}
