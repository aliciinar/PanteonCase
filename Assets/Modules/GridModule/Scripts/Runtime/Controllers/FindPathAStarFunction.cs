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
    /// The shortest walk between two cells, by A*: every cell from start to goal, both included, or null when the
    /// goal cannot be reached. A walker steps to the four neighbours of a cell and cannot step onto a cell whose
    /// occupant is of the blocked kind - which kind is the caller's rule, so the grid still knows no building or
    /// unit. Other occupants are walked through. The start is never checked: a unit leaves its building from a
    /// cell of that building.
    ///
    /// Each open cell is scored f = g + h: g the steps walked from the start, h the Manhattan distance still to
    /// go, which never overestimates a four-neighbour walk, so the first time the goal is taken from the open set
    /// its walk is a shortest one. Among equal f the cell nearer the goal goes first, which keeps walks straight.
    /// The open set is a SortedSet of (f, h, order) - .NET Standard 2.1 has no priority queue - and a cell reached
    /// again by a shorter walk is simply added again; the stale entry is skipped when it comes out, because the
    /// cell is closed by then.
    ///
    /// The function provider pools this function, so its working sets are kept and cleared on every run rather than
    /// made anew for each order; only the walk it returns is new.
    /// </summary>
    internal class FindPathAStarFunction : FunctionReturn<List<Vector2Int>, Vector2Int, Vector2Int, CellOccupantType>
    {
        [Inject] private IGridService _gridService { get; set; }

        private readonly SortedSet<(int f, int h, int order)> _open = new();
        private readonly Dictionary<int, Vector2Int> _openCells = new();
        private readonly Dictionary<Vector2Int, int> _walked = new();
        private readonly Dictionary<Vector2Int, Vector2Int> _cameFrom = new();
        private readonly HashSet<Vector2Int> _closed = new();

        public override List<Vector2Int> Execute(Vector2Int start, Vector2Int goal, CellOccupantType blockedBy)
        {
            CellVO[,] cells = _gridService.Cells;

            _open.Clear();
            _openCells.Clear();
            _walked.Clear();
            _cameFrom.Clear();
            _closed.Clear();
            int order = 0;

            _walked[start] = 0;
            int startH = _gridService.Steps(start, goal);
            _open.Add((startH, startH, order));
            _openCells[order++] = start;

            while (_open.Count > 0)
            {
                (int f, int h, int order) best = _open.Min;
                _open.Remove(best);
                Vector2Int cell = _openCells[best.order];
                _openCells.Remove(best.order);

                if (!_closed.Add(cell)) continue;
                if (cell == goal) return Walk(cell);

                foreach (Vector2Int step in GridConstants.NeighbourSteps)
                {
                    Vector2Int next = cell + step;
                    if (!_gridService.IsInside(next) || _closed.Contains(next)) continue;
                    if (IsBlocked(cells[next.x, next.y], blockedBy)) continue;

                    int nextWalked = _walked[cell] + 1;
                    if (_walked.TryGetValue(next, out int known) && nextWalked >= known) continue;

                    _walked[next] = nextWalked;
                    _cameFrom[next] = cell;

                    int nextH = _gridService.Steps(next, goal);
                    _open.Add((nextWalked + nextH, nextH, order));
                    _openCells[order++] = next;
                }
            }

            return null;
        }

        private static bool IsBlocked(CellVO cell, CellOccupantType blockedBy) => !cell.IsFree && cell.Occupant.Kind == blockedBy;

        private List<Vector2Int> Walk(Vector2Int goal)
        {
            var walk = new List<Vector2Int> { goal };
            Vector2Int cell = goal;

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
