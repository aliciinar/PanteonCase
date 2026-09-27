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
    /// The free cell nearest to a cell - the cell itself when it is free - or null when no cell of the grid is free.
    ///
    /// Breadth-first search from the source: its neighbours are one step away, theirs two - so the first free cell
    /// found is as few steps from the source as any free cell can be. Taken cells are searched through, not around:
    /// this is the nearest cell, not the nearest cell by walking. Among free cells equally far, the one reached
    /// through the earlier neighbour step (up, right, down, left) wins.
    /// </summary>
    internal class FindNearestFreeCellBfsFunction : FunctionReturn<Vector2Int?, Vector2Int>
    {
        [Inject] private IGridService _gridService { get; set; }

        public override Vector2Int? Execute(Vector2Int source)
        {
            CellVO[,] cells = _gridService.Cells;

            var frontier = new Queue<Vector2Int>();
            var visited = new HashSet<Vector2Int> { source };
            frontier.Enqueue(source);

            while (frontier.Count > 0)
            {
                Vector2Int cell = frontier.Dequeue();

                if (_gridService.IsInside(cell) && cells[cell.x, cell.y].IsFree) return cell;

                foreach (Vector2Int step in GridConstants.NeighbourSteps)
                {
                    Vector2Int next = cell + step;
                    if (_gridService.IsInside(next) && visited.Add(next)) frontier.Enqueue(next);
                }
            }

            return null;
        }
    }
}
