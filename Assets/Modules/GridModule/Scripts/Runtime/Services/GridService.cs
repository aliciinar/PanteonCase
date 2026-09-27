using System.Collections.Generic;
using FlowIoC.BaseModule.Function.Provider;
using FlowIoC.BaseModule.Injectable.Attributes;
using Modules.GridModule.Controllers;
using Modules.GridModule.Data.ValueObjects;
using Modules.GridModule.Models;
using Modules.GridModule.Shared.Data.ValueObjects;
using Modules.GridModule.Shared.Enums;
using UnityEngine;

namespace Modules.GridModule.Services
{
    /// <summary>
    /// Answers the grid's questions. Lookups and geometry are answered here - some run once per cell of a search
    /// or once per frame of a drag, too often to go through the function provider. The searches are Functions in
    /// Controllers/, one per algorithm.
    /// </summary>
    internal class GridService : IGridService
    {
        [Inject] private IGridModel _gridModel { get; set; }
        [Inject] private IFunctionProvider _functionProvider { get; set; }

        public Vector2Int GridSize => _gridModel.GridSize;
        public float CellSize => _gridModel.CellSize;
        public Rect Bounds => _gridModel.Bounds;
        public CellVO[,] Cells => _gridModel.Cells;

        public void Build(Vector2Int gridSize, float cellSize)
        {
            Vector2 worldSize = new Vector2(gridSize.x, gridSize.y) * cellSize;

            _gridModel.GridSize = gridSize;
            _gridModel.CellSize = cellSize;
            _gridModel.Bounds = new Rect(-worldSize * 0.5f, worldSize);

            // Every cell starts free. The asset outlives the play session, so this replaces whatever
            // the last session left in it.
            _gridModel.Cells = CreateCells();
        }

        public Vector2Int WorldToCell(Vector2 worldPosition)
        {
            Rect bounds = _gridModel.Bounds;
            float cellSize = _gridModel.CellSize;
            return new Vector2Int(Mathf.FloorToInt((worldPosition.x - bounds.xMin) / cellSize),
                                  Mathf.FloorToInt((worldPosition.y - bounds.yMin) / cellSize));
        }

        public bool IsInside(Vector2Int cell)
        {
            Vector2Int gridSize = _gridModel.GridSize;
            return cell.x >= 0 && cell.y >= 0 && cell.x < gridSize.x && cell.y < gridSize.y;
        }

        public Rect AreaToWorldRect(RectInt area)
        {
            float cellSize = _gridModel.CellSize;
            return new Rect(_gridModel.Bounds.min + (Vector2)area.position * cellSize, (Vector2)area.size * cellSize);
        }

        public bool IsAreaFree(RectInt area)
        {
            Vector2Int gridSize = _gridModel.GridSize;

            if (area.xMin < 0 || area.yMin < 0 || area.xMax > gridSize.x || area.yMax > gridSize.y)
                return false;

            CellVO[,] cells = _gridModel.Cells;

            foreach (Vector2Int cell in area.allPositionsWithin)
            {
                if (!cells[cell.x, cell.y].IsFree) return false;
            }

            return true;
        }

        public Vector2Int CentredOrigin(Vector2Int size)
        {
            Vector2Int gridSize = _gridModel.GridSize;
            return new Vector2Int(Mathf.Max(0, (gridSize.x - size.x) / 2), Mathf.Max(0, (gridSize.y - size.y) / 2));
        }

        public Vector2Int ClampArea(Vector2Int origin, Vector2Int size)
        {
            Vector2Int gridSize = _gridModel.GridSize;
            return new Vector2Int(Mathf.Clamp(origin.x, 0, Mathf.Max(0, gridSize.x - size.x)),
                                  Mathf.Clamp(origin.y, 0, Mathf.Max(0, gridSize.y - size.y)));
        }

        public void Occupy(RectInt area, CellOccupantVO occupant)
        {
            CellVO[,] cells = _gridModel.Cells;
            foreach (Vector2Int cell in area.allPositionsWithin)
                cells[cell.x, cell.y].Occupant = occupant;
        }

        public void MoveOccupant(BoardUnitVO unit, Vector2Int to)
        {
            CellVO[,] cells = _gridModel.Cells;
            cells[unit.Cell.x, unit.Cell.y].Occupant = null;
            cells[to.x, to.y].Occupant = unit;
            unit.Cell = to;
        }

        public void Remove(CellOccupantVO occupant)
        {
            CellVO[,] cells = _gridModel.Cells;
            foreach (Vector2Int cell in occupant.Area.allPositionsWithin)
                cells[cell.x, cell.y].Occupant = null;
        }

        public Vector2Int? FindFreeCellAroundBfs(RectInt area, Vector2Int towards) =>
            _functionProvider.Call<FindFreeCellAroundBfsFunction>().AddParams(area, towards)
                             .ExecuteAndGetResult<Vector2Int?>();

        public Vector2Int? FindNearestFreeAreaBfs(Vector2Int size) =>
            _functionProvider.Call<FindNearestFreeAreaBfsFunction>().AddParams(size)
                             .ExecuteAndGetResult<Vector2Int?>();

        public Vector2Int? FindNearestFreeCellBfs(Vector2Int source) =>
            _functionProvider.Call<FindNearestFreeCellBfsFunction>().AddParams(source)
                             .ExecuteAndGetResult<Vector2Int?>();

        public List<Vector2Int> FindPathAStar(Vector2Int start, Vector2Int goal, CellOccupantType blockedBy) =>
            _functionProvider.Call<FindPathAStarFunction>().AddParams(start, goal, blockedBy)
                             .ExecuteAndGetResult<List<Vector2Int>>();

        private CellVO[,] CreateCells()
        {
            Vector2Int gridSize = _gridModel.GridSize;
            float cellSize = _gridModel.CellSize;
            Rect bounds = _gridModel.Bounds;
            var cells = new CellVO[gridSize.x, gridSize.y];

            for (int column = 0; column < gridSize.x; column++)
            {
                for (int row = 0; row < gridSize.y; row++)
                {
                    var centre = new Vector2(bounds.xMin + (column + 0.5f) * cellSize,
                                             bounds.yMin + (row + 0.5f) * cellSize);
                    cells[column, row] = new CellVO(centre);
                }
            }

            return cells;
        }
    }
}
