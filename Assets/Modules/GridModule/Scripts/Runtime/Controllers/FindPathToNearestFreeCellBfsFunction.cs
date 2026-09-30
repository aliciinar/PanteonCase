using System.Collections.Generic;
using FlowIoC.BaseModule.Function.ReturnableFunctions;
using FlowIoC.BaseModule.Injectable.Attributes;
using Modules.GridModule.Constants;
using Modules.GridModule.Data.ValueObjects;
using Modules.GridModule.Services;
using Modules.GridModule.Shared.Enums;
using UnityEngine;

namespace Modules.GridModule.Controllers
{
    /// <summary>
    /// The shortest walk from a cell to the free cell nearest a target that can be walked to: every cell from start
    /// to that cell, both included, or null when no free cell can be walked to. The target itself wins when it is
    /// free and reachable. It is where a unit out of a door goes: the spawn point, or when that is taken the free
    /// cell nearest it - never one walled off from the door.
    ///
    /// Breadth-first search out of the start, walking as A* does: four neighbours, never onto a cell whose occupant
    /// is of the blocked kind, other occupants walked through, the start never checked (a door is a cell of its
    /// building). Every free cell reached is a candidate, and the one fewest Manhattan steps from the target wins; on
    /// a tie, the one reached first - the shorter walk. No free cell further out can be ruled out before the walk
    /// reaches it, so the search ends only when the target is reached free or nothing is left to reach. Every step
    /// costs the same, so the order a breadth-first search reaches a cell in gives the shortest walk to it, kept
    /// as each cell's previous cell.
    /// </summary>
    internal class FindPathToNearestFreeCellBfsFunction : FunctionReturn<List<Vector2Int>, Vector2Int, Vector2Int, CellOccupantType>
    {
        [Inject] private IGridService _gridService { get; set; }

        /// <summary>Kept across runs - the function provider pools this function - and cleared on each; only the walk it returns is new.</summary>
        private readonly Queue<Vector2Int> _frontier = new();
        private readonly Dictionary<Vector2Int, Vector2Int> _cameFrom = new();
        private readonly HashSet<Vector2Int> _visited = new();

        public override List<Vector2Int> Execute(Vector2Int start, Vector2Int target, CellOccupantType blockedBy)
        {
            CellVO[,] cells = _gridService.Cells;

            _frontier.Clear();
            _cameFrom.Clear();
            _visited.Clear();
            _visited.Add(start);
            _frontier.Enqueue(start);

            Vector2Int? best = null;
            int bestSteps = int.MaxValue;

            while (_frontier.Count > 0)
            {
                Vector2Int cell = _frontier.Dequeue();

                foreach (Vector2Int step in GridConstants.NeighbourSteps)
                {
                    Vector2Int next = cell + step;
                    if (!_gridService.IsInside(next) || !_visited.Add(next)) continue;

                    CellVO nextCell = cells[next.x, next.y];
                    if (!nextCell.IsFree && nextCell.Occupant.Kind == blockedBy) continue;

                    _cameFrom[next] = cell;
                    _frontier.Enqueue(next);

                    if (!nextCell.IsFree) continue;

                    int steps = _gridService.Steps(next, target);
                    if (steps >= bestSteps) continue;

                    best = next;
                    bestSteps = steps;
                    if (steps == 0) return Walk(next);
                }
            }

            return best == null ? null : Walk(best.Value);
        }

        private List<Vector2Int> Walk(Vector2Int end)
        {
            var walk = new List<Vector2Int> { end };
            Vector2Int cell = end;

            while (_cameFrom.TryGetValue(cell, out Vector2Int previous))
            {
                walk.Add(previous);
                cell = previous;
            }

            walk.Reverse();
            return walk;
        }
    }
}
