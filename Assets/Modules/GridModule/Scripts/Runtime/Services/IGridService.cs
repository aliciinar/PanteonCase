using System.Collections.Generic;
using Modules.GridModule.Data.ValueObjects;
using Modules.GridModule.Shared.Data.ValueObjects;
using Modules.GridModule.Shared.Enums;
using UnityEngine;

namespace Modules.GridModule.Services
{
    /// <summary>
    /// The board's grid, everything standing on it, and the questions every module asks of them. The grid
    /// is centred on the world origin; cell [0, 0] is the bottom-left one. What stands on a cell is a
    /// CellOccupantVO - a BoardBuildingVO or a BoardUnitVO, the only copy of that thing's data - and the
    /// grid is the only thing that writes what occupies a cell. A search names its algorithm: Bfs and AStar
    /// are what runs.
    /// </summary>
    public interface IGridService
    {
        /// <summary>Columns (x) and rows (y).</summary>
        Vector2Int GridSize { get; }

        /// <summary>Edge of one cell in world units.</summary>
        float CellSize { get; }

        /// <summary>The world rect the cells cover.</summary>
        Rect Bounds { get; }

        /// <summary>Every cell, indexed [column, row], each with what stands on it.</summary>
        CellVO[,] Cells { get; }

        /// <summary>Lays out an empty grid of this size, centred on the world origin, replacing any before it.</summary>
        void Build(Vector2Int gridSize, float cellSize);

        /// <summary>The cell a world position falls in. May lie outside the grid - check with IsInside.</summary>
        Vector2Int WorldToCell(Vector2 worldPosition);

        bool IsInside(Vector2Int cell);

        /// <summary>How many four-neighbour steps lie between two cells - their Manhattan distance, obstacles ignored.</summary>
        int Steps(Vector2Int from, Vector2Int to);

        /// <summary>Whether the cell is outside the area and one neighbour step from one of its cells.</summary>
        bool IsNextTo(Vector2Int cell, RectInt area);

        /// <summary>The world rect an area of cells covers.</summary>
        Rect AreaToWorldRect(RectInt area);

        /// <summary>Whether the area lies entirely inside the grid and nothing stands on any of its cells.</summary>
        bool IsAreaFree(RectInt area);

        /// <summary>The bottom-left cell that centres an area of this size on the grid.</summary>
        Vector2Int CentredOrigin(Vector2Int size);

        /// <summary>The nearest bottom-left cell that keeps an area of this size inside the grid.</summary>
        Vector2Int ClampArea(Vector2Int origin, Vector2Int size);

        /// <summary>Puts the occupant on every cell of the area - the same instance on each, so any cell it covers answers for it.</summary>
        void Occupy(RectInt area, CellOccupantVO occupant);

        /// <summary>Moves the unit to another cell - the same instance, now standing there - and frees the cell it held.</summary>
        void MoveOccupant(BoardUnitVO unit, Vector2Int to);

        /// <summary>Takes the occupant off the board: every cell it covered is free again.</summary>
        void Remove(CellOccupantVO occupant);

        /// <summary>
        /// The free cell next to the area - one of the four steps off one of its cells - nearest <paramref name="towards"/>,
        /// by breadth-first search out of the area; null when every cell next to it is taken or off the grid.
        /// </summary>
        Vector2Int? FindFreeCellAroundBfs(RectInt area, Vector2Int towards);

        /// <summary>The bottom-left cell of the free area of this size nearest the grid's centre, by breadth-first search; null when it fits nowhere.</summary>
        Vector2Int? FindNearestFreeAreaBfs(Vector2Int size);

        /// <summary>The free cell nearest this one, by breadth-first search - the cell itself when it is free; null when no cell is free.</summary>
        Vector2Int? FindNearestFreeCellBfs(Vector2Int source);

        /// <summary>
        /// The shortest four-neighbour walk from <paramref name="start"/> to <paramref name="goal"/> by A*, both
        /// included, never stepping on a cell whose occupant is <paramref name="blockedBy"/>; null when the goal
        /// cannot be reached.
        /// </summary>
        List<Vector2Int> FindPathAStar(Vector2Int start, Vector2Int goal, CellOccupantType blockedBy);
    }
}
