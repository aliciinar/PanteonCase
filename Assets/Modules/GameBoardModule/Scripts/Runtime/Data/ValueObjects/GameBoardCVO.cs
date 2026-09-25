using System;
using UnityEngine;

namespace Modules.GameBoardModule.Data.ValueObjects
{
    /// <summary>
    /// The board as the designer authors it: how many cells it has, how big one cell is, and how
    /// much room the frame leaves around the grid.
    /// </summary>
    [Serializable]
    public class GameBoardCVO
    {
        [Tooltip("Columns (x) and rows (y) of the grid.")]
        [Min(1)] public Vector2Int GridSize = new(24, 16);

        [Tooltip("Edge of one square cell in pixels. The brief fixes it at 32.")]
        [Min(1)] public int CellPixelSize = 32;

        [Tooltip("Pixels per world unit the board's sprites are imported at. A cell is CellPixelSize / PixelsPerUnit world units.")]
        [Min(1)] public float PixelsPerUnit = 32f;

        [Tooltip("Gap between the grid and the frame drawn around it, in cells.")]
        [Min(0)] public float FramePaddingInCells = 1f;
    }
}
