using FlowIoC.BaseModule.Injectable.Components;
using FlowIoC.BaseModule.ViewsMediators.View;
using Modules.GameBoardModule.Entities;
using Modules.GridModule.Data.ValueObjects;
using UnityEngine;

namespace Modules.GameBoardModule.ViewsMediators
{
    /// <summary>
    /// The board in the scene: a 9-sliced frame and a grey sprite per cell, parented under Cells, whose
    /// darker edges draw the grid. The cell sprites are pooled and share one sprite and one material, so
    /// the grid batches together. Sizes and positions come from the layout BuildGameBoardCommand computes.
    /// </summary>
    [RequireComponent(typeof(ViewInjector))]
    public class GameBoardView : MonoBehaviour, IView
    {
        public bool IsRegistered { get; set; }

        [SerializeField] private SpriteRenderer _frame;

        [Tooltip("Parent of the cell sprites.")]
        [SerializeField] private Transform _cells;

        /// <summary>The cells last drawn, indexed [column, row]. Empty until the board is built. Read by the editor gizmos.</summary>
        internal CellVO[,] Cells { get; private set; } = new CellVO[0, 0];

        /// <summary>Edge of one cell in world units.</summary>
        internal float CellSize { get; private set; }

        /// <param name="frameBounds">World rect the frame fills.</param>
        /// <param name="cellSize">Edge of one cell in world units.</param>
        /// <param name="cells">Every cell, indexed [column, row].</param>
        /// <param name="tiles">A pooled cell sprite for every cell, indexed like cells.</param>
        public void Draw(Rect frameBounds, float cellSize, CellVO[,] cells, BoardCell[,] tiles)
        {
            Cells = cells;
            CellSize = cellSize;

            _frame.drawMode = SpriteDrawMode.Sliced;
            _frame.transform.position = frameBounds.center;
            _frame.size = frameBounds.size;

            for (int column = 0; column < cells.GetLength(0); column++)
            {
                for (int row = 0; row < cells.GetLength(1); row++)
                {
                    BoardCell tile = tiles[column, row];
                    tile.name = $"Cell {column},{row}";
                    tile.transform.SetParent(_cells, false);
                    tile.Show(cells[column, row].Position, cellSize);
                }
            }
        }
    }
}
