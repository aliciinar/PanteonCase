using FlowIoC.BaseModule.Injectable.Components;
using FlowIoC.BaseModule.ViewsMediators.View;
using Modules.GameBoardModule.Shared.Data.ValueObjects;
using UnityEngine;

namespace Modules.GameBoardModule.ViewsMediators
{
    /// <summary>
    /// The board in the scene: a 9-sliced frame and a tiled grid, two sprite renderers in total so
    /// the whole board costs next to nothing in draw calls. Sizes and positions come from the model.
    /// </summary>
    [RequireComponent(typeof(ViewInjector))]
    public class GameBoardView : MonoBehaviour, IView
    {
        public bool IsRegistered { get; set; }

        [SerializeField] private SpriteRenderer _frame;
        [SerializeField] private SpriteRenderer _grid;

        /// <summary>The cells last drawn, indexed [column, row]. Empty until the board is built.</summary>
        internal CellVO[,] Cells { get; private set; } = new CellVO[0, 0];

        /// <summary>Edge of one cell in world units.</summary>
        internal float CellSize { get; private set; }

        /// <param name="gridBounds">World rect the cells cover.</param>
        /// <param name="frameBounds">World rect the frame fills.</param>
        /// <param name="cellSize">Edge of one cell in world units; the grid sprite tiles once per cell.</param>
        /// <param name="cells">Every cell, indexed [column, row].</param>
        public void Draw(Rect gridBounds, Rect frameBounds, float cellSize, CellVO[,] cells)
        {
            Cells = cells;
            CellSize = cellSize;

            _frame.drawMode = SpriteDrawMode.Sliced;
            _frame.transform.position = frameBounds.center;
            _frame.size = frameBounds.size;

            // The grid sprite is one cell at scale 1, so the renderer is scaled to the cell size and
            // sized in cells - which keeps one tile per cell whatever CellPixelSize says.
            _grid.drawMode = SpriteDrawMode.Tiled;
            _grid.transform.position = gridBounds.center;
            _grid.transform.localScale = Vector3.one * cellSize;
            _grid.size = gridBounds.size / cellSize;
        }
    }
}
