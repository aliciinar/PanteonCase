using System.Collections.Generic;
using FlowIoC.BaseModule.Injectable.Attributes;
using Modules.GridModule.Models;
using Modules.GridModule.Shared.Data.ValueObjects;
using Modules.GridModule.Shared.Enums;
using UnityEngine;

namespace Modules.GridModule.Services
{
    public class GridService : IGridService
    {
        private static readonly Vector2Int[] NeighbourSteps =
        {
            Vector2Int.up, Vector2Int.right, Vector2Int.down, Vector2Int.left
        };

        [Inject] private IGridModel _gridModel { get; set; }

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

        /// <summary>
        /// Breadth-first search over the grid. Each cell is tried as the area's bottom-left corner, starting
        /// from the corner that would centre the area on the grid, then its four neighbours, then theirs -
        /// so corners are tried in order of how many steps they are from the centred position, and the
        /// first one where the whole area is free is the nearest fit. If the search runs out of cells,
        /// nothing fits anywhere. An area larger than the grid starts at 0 and is rejected everywhere.
        /// </summary>
        public Vector2Int? FindFreeArea(Vector2Int size)
        {
            Vector2Int start = CentredOrigin(size);

            var frontier = new Queue<Vector2Int>();
            var visited = new HashSet<Vector2Int>();
            frontier.Enqueue(start);
            visited.Add(start);

            while (frontier.Count > 0)
            {
                Vector2Int cell = frontier.Dequeue();

                if (IsAreaFree(new RectInt(cell, size))) return cell;

                foreach (Vector2Int step in NeighbourSteps)
                {
                    Vector2Int next = cell + step;
                    if (IsInside(next) && visited.Add(next)) frontier.Enqueue(next);
                }
            }

            return null;
        }

        public Vector2Int ClampArea(Vector2Int origin, Vector2Int size)
        {
            Vector2Int gridSize = _gridModel.GridSize;
            return new Vector2Int(Mathf.Clamp(origin.x, 0, Mathf.Max(0, gridSize.x - size.x)),
                                  Mathf.Clamp(origin.y, 0, Mathf.Max(0, gridSize.y - size.y)));
        }

        public int Occupy(RectInt area, CellOccupantType type)
        {
            _gridModel.LastEntityId++;
            var occupant = new CellOccupantVO(_gridModel.LastEntityId, type);

            CellVO[,] cells = _gridModel.Cells;
            foreach (Vector2Int cell in area.allPositionsWithin)
                cells[cell.x, cell.y].Occupant = occupant;

            return occupant.EntityId;
        }

        // detayına bakılacak.
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
