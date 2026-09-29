using System.Collections.Generic;
using FlowIoC.BaseModule.Function.ReturnableFunctions;
using FlowIoC.BaseModule.Injectable.Attributes;
using Modules.GridModule.Constants;
using Modules.GridModule.Data.ValueObjects;
using Modules.GridModule.Services;
using UnityEngine;

namespace Modules.GridModule.Controllers
{
    /// <summary>
    /// The free cell right next to an area - one neighbour step off one of its cells, outside it - nearest a given
    /// cell, or null when every cell next to the area is taken or off the grid. It is where a unit stands to strike
    /// what covers the area: a strike reaches one cell, so the search looks no further than the first ring.
    ///
    /// Breadth-first search out of the area: every cell of the area is a source at step 0, so the first step out
    /// finds exactly the ring around it, whatever the area's size. Among the free cells of that ring the one fewest
    /// Manhattan steps from <c>towards</c> wins - the side of a building its attacker comes from; on a tie, the one
    /// found first (up, right, down, left from the earlier area cell).
    /// </summary>
    internal class FindFreeCellAroundBfsFunction : FunctionReturn<Vector2Int?, RectInt, Vector2Int>
    {
        [Inject] private IGridService _gridService { get; set; }

        /// <summary>Kept across runs - the function provider pools this function - and cleared on each.</summary>
        private readonly HashSet<Vector2Int> _visited = new();

        public override Vector2Int? Execute(RectInt area, Vector2Int towards)
        {
            CellVO[,] cells = _gridService.Cells;

            HashSet<Vector2Int> visited = _visited;
            visited.Clear();
            foreach (Vector2Int cell in area.allPositionsWithin)
                visited.Add(cell);

            Vector2Int? best = null;
            int bestDistance = int.MaxValue;

            foreach (Vector2Int cell in area.allPositionsWithin)
            {
                foreach (Vector2Int step in GridConstants.NeighbourSteps)
                {
                    Vector2Int next = cell + step;
                    if (!visited.Add(next) || !_gridService.IsInside(next) || !cells[next.x, next.y].IsFree) continue;

                    int distance = _gridService.Steps(next, towards);
                    if (distance >= bestDistance) continue;

                    best = next;
                    bestDistance = distance;
                }
            }

            return best;
        }
    }
}
