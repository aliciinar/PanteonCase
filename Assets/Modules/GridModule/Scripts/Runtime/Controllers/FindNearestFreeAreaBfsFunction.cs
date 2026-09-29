using System.Collections.Generic;
using FlowIoC.BaseModule.Function.ReturnableFunctions;
using FlowIoC.BaseModule.Injectable.Attributes;
using Modules.GridModule.Constants;
using Modules.GridModule.Services;
using UnityEngine;

namespace Modules.GridModule.Controllers
{
    /// <summary>
    /// The bottom-left cell of the free area of this size nearest the grid's centre, or null when it fits nowhere.
    ///
    /// Breadth-first search over the grid. Each cell is tried as the area's bottom-left corner, starting from the
    /// corner that would centre the area on the grid, then its four neighbours, then theirs - so corners are tried
    /// in order of how many steps they are from the centred position, and the first one where the whole area is
    /// free is the nearest fit. If the search runs out of cells, nothing fits anywhere. An area larger than the
    /// grid starts at 0 and is rejected everywhere.
    /// </summary>
    internal class FindNearestFreeAreaBfsFunction : FunctionReturn<Vector2Int?, Vector2Int>
    {
        [Inject] private IGridService _gridService { get; set; }

        /// <summary>Kept across runs - the function provider pools this function - and cleared on each.</summary>
        private readonly Queue<Vector2Int> _frontier = new();
        private readonly HashSet<Vector2Int> _visited = new();

        public override Vector2Int? Execute(Vector2Int size)
        {
            Vector2Int start = _gridService.CentredOrigin(size);

            Queue<Vector2Int> frontier = _frontier;
            HashSet<Vector2Int> visited = _visited;
            frontier.Clear();
            visited.Clear();
            visited.Add(start);
            frontier.Enqueue(start);

            while (frontier.Count > 0)
            {
                Vector2Int corner = frontier.Dequeue();

                if (_gridService.IsAreaFree(new RectInt(corner, size))) return corner;

                foreach (Vector2Int step in GridConstants.NeighbourSteps)
                {
                    Vector2Int next = corner + step;
                    if (_gridService.IsInside(next) && visited.Add(next)) frontier.Enqueue(next);
                }
            }

            return null;
        }
    }
}
