using System;
using FlowIoC.BaseModule.Injectable.Components;
using FlowIoC.BaseModule.ViewsMediators.View;
using Modules.GridModule.Data.ValueObjects;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Modules.GameBoardModule.ViewsMediators
{
    /// <summary>
    /// The board in the scene: a 9-sliced frame, and a tilemap with one tile of the cell sprite per cell, whose darker
    /// edges draw the grid. The tilemap is one mesh, built once when the board is drawn and never again - no object
    /// per cell. Sizes and positions come from the layout BuildGameBoardCommand computes.
    /// </summary>
    [RequireComponent(typeof(ViewInjector))]
    public class GameBoardView : MonoBehaviour, IView
    {
        public bool IsRegistered { get; set; }

        [SerializeField] private SpriteRenderer _frame;

        [Tooltip("Lays the tiles out: moved to the grid's bottom-left corner and scaled so that one tile is one cell.")]
        [SerializeField] private Grid _grid;

        [Tooltip("Holds a tile for every cell, under the grid.")]
        [SerializeField] private Tilemap _tilemap;

        /// <summary>The one tile every cell shows; made when the board is drawn.</summary>
        private Tile _tile;

        /// <summary>The cells last drawn, indexed [column, row]. Empty until the board is built. Read by the editor gizmos.</summary>
        internal CellVO[,] Cells { get; private set; } = new CellVO[0, 0];

        /// <summary>Edge of one cell in world units.</summary>
        internal float CellSize { get; private set; }

        /// <param name="gridBounds">World rect the cells cover.</param>
        /// <param name="frameBounds">World rect the frame fills.</param>
        /// <param name="cellSize">Edge of one cell in world units.</param>
        /// <param name="cells">Every cell, indexed [column, row].</param>
        /// <param name="cellSprite">The square every cell is drawn with, scaled to the cell.</param>
        public void Draw(Rect gridBounds, Rect frameBounds, float cellSize, CellVO[,] cells, Sprite cellSprite)
        {
            Cells = cells;
            CellSize = cellSize;

            _frame.drawMode = SpriteDrawMode.Sliced;
            _frame.transform.position = frameBounds.center;
            _frame.size = frameBounds.size;

            // A tile is drawn at its sprite's own size; the grid's cells are made that size, then the grid is scaled
            // so that one of them is one board cell.
            float spriteSize = cellSprite.bounds.size.x;
            _grid.cellSize = new Vector3(spriteSize, spriteSize, 0f);
            _grid.transform.position = gridBounds.min;
            _grid.transform.localScale = Vector3.one * (cellSize / spriteSize);

            _tile = ScriptableObject.CreateInstance<Tile>();
            _tile.sprite = cellSprite;

            int columns = cells.GetLength(0);
            int rows = cells.GetLength(1);
            var tiles = new TileBase[columns * rows];
            Array.Fill(tiles, _tile);
            _tilemap.SetTilesBlock(new BoundsInt(0, 0, 0, columns, rows, 1), tiles);
        }

        private void OnDestroy() => Destroy(_tile);
    }
}
