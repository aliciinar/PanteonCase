using System.Collections.Generic;
using FlowIoC.BaseModule.Function.ReturnableFunctions;
using FlowIoC.BaseModule.Injectable.Attributes;
using Modules.GridModule.Constants;
using Modules.GridModule.Data.ValueObjects;
using Modules.GridModule.Enums;
using Modules.GridModule.Services;
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
    /// </summary>
    internal class FindPathAStarFunction : FunctionReturn<List<Vector2Int>, Vector2Int, Vector2Int, CellOccupantType>
    {
        [Inject] private IGridService _gridService { get; set; }

        public override List<Vector2Int> Execute(Vector2Int start, Vector2Int goal, CellOccupantType blockedBy)
        {
            CellVO[,] cells = _gridService.Cells;

            var open = new SortedSet<(int f, int h, int order)>();
            var openCells = new Dictionary<int, Vector2Int>();
            var walked = new Dictionary<Vector2Int, int> { [start] = 0 };
            var cameFrom = new Dictionary<Vector2Int, Vector2Int>();
            var closed = new HashSet<Vector2Int>();
            int order = 0;

            int startH = Distance(start, goal);
            open.Add((startH, startH, order));
            openCells[order++] = start;

            while (open.Count > 0)
            {
                (int f, int h, int order) best = open.Min;
                open.Remove(best);
                Vector2Int cell = openCells[best.order];
                openCells.Remove(best.order);

                if (!closed.Add(cell)) continue;
                if (cell == goal) return Walk(cameFrom, cell);

                foreach (Vector2Int step in GridConstants.NeighbourSteps)
                {
                    Vector2Int next = cell + step;
                    if (!_gridService.IsInside(next) || closed.Contains(next)) continue;
                    if (IsBlocked(cells[next.x, next.y], blockedBy)) continue;

                    int nextWalked = walked[cell] + 1;
                    if (walked.TryGetValue(next, out int known) && nextWalked >= known) continue;

                    walked[next] = nextWalked;
                    cameFrom[next] = cell;

                    int nextH = Distance(next, goal);
                    open.Add((nextWalked + nextH, nextH, order));
                    openCells[order++] = next;
                }
            }

            return null;
        }

        private static bool IsBlocked(CellVO cell, CellOccupantType blockedBy) => !cell.IsFree && cell.Occupant.Type == blockedBy;

        private static int Distance(Vector2Int a, Vector2Int b) => Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);

        private static List<Vector2Int> Walk(Dictionary<Vector2Int, Vector2Int> cameFrom, Vector2Int goal)
        {
            var walk = new List<Vector2Int> { goal };
            Vector2Int cell = goal;

            while (cameFrom.TryGetValue(cell, out Vector2Int previous))
            {
                walk.Add(previous);
                cell = previous;
            }

            walk.Reverse();
            return walk;
        }
    }
}
