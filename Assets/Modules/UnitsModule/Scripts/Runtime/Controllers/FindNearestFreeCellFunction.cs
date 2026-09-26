using System.Collections.Generic;
using FlowIoC.BaseModule.Function.ReturnableFunctions;
using FlowIoC.BaseModule.Injectable.Attributes;
using Modules.GridModule.Services;
using Modules.GridModule.Shared.Data.ValueObjects;
using UnityEngine;

namespace Modules.UnitsModule.Controllers
{
    /// <summary>
    /// The free cell nearest to a set of cells, or null when no cell of the grid is free.
    ///
    /// Breadth-first search from every source at once: the sources are one step away from nothing, their
    /// neighbours one step, theirs two - so the first free cell found is as few steps from the nearest
    /// source as any free cell can be. Taken cells are searched through, not around: this is the nearest
    /// cell, not the nearest cell by walking. Among free cells equally far, the one reached from an
    /// earlier source wins, so callers list the sources they prefer first.
    ///
    /// A unit whose spawn cell is taken passes that cell alone.
    /// </summary>
    internal class FindNearestFreeCellFunction : FunctionReturn<Vector2Int?, List<Vector2Int>>
    {
        private static readonly Vector2Int[] NeighbourSteps =
        {
            Vector2Int.up, Vector2Int.right, Vector2Int.down, Vector2Int.left
        };

        [Inject] private IGridService _gridService { get; set; }

        public override Vector2Int? Execute(List<Vector2Int> sources)
        {
            CellVO[,] cells = _gridService.Cells;
            var frontier = new Queue<Vector2Int>();
            var visited = new HashSet<Vector2Int>();

            foreach (Vector2Int source in sources)
            {
                if (visited.Add(source)) frontier.Enqueue(source);
            }

            while (frontier.Count > 0)
            {
                Vector2Int cell = frontier.Dequeue();

                if (_gridService.IsInside(cell) && cells[cell.x, cell.y].IsFree) return cell;

                foreach (Vector2Int step in NeighbourSteps)
                {
                    Vector2Int next = cell + step;
                    if (_gridService.IsInside(next) && visited.Add(next)) frontier.Enqueue(next);
                }
            }

            return null;
        }
    }
}
