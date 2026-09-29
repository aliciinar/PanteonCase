using Modules.GridModule.Data.ValueObjects;
using UnityEngine;

namespace Modules.GameBoardModule.Data.ValueObjects
{
    /// <summary>What the board view needs to draw the board, in world units. Computed by BuildGameBoardCommand.</summary>
    internal readonly struct GameBoardLayoutVO
    {
        /// <summary>The area the cells cover.</summary>
        public readonly Rect GridBounds;

        /// <summary>The grid plus the padding the frame sits in.</summary>
        public readonly Rect FrameBounds;

        /// <summary>Edge of one cell in world units.</summary>
        public readonly float CellSize;

        /// <summary>Every cell, indexed [column, row].</summary>
        public readonly CellVO[,] Cells;

        /// <summary>The square every cell is drawn with.</summary>
        public readonly Sprite CellSprite;

        public GameBoardLayoutVO(Rect gridBounds, Rect frameBounds, float cellSize, CellVO[,] cells, Sprite cellSprite)
        {
            GridBounds = gridBounds;
            FrameBounds = frameBounds;
            CellSize = cellSize;
            Cells = cells;
            CellSprite = cellSprite;
        }
    }
}
