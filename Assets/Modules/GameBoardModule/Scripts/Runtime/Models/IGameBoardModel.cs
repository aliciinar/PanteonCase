using UnityEngine;

namespace Modules.GameBoardModule.Models
{
    /// <summary>The board as CD_GameBoard authors it, with the cell size already in world units.</summary>
    internal interface IGameBoardModel
    {
        /// <summary>Columns (x) and rows (y).</summary>
        Vector2Int GridSize { get; }

        /// <summary>Edge of one cell in world units.</summary>
        float CellSize { get; }

        /// <summary>Gap between the grid and the frame drawn around it, in cells.</summary>
        float FramePaddingInCells { get; }

        /// <summary>The square every cell is drawn with.</summary>
        Sprite CellSprite { get; }
    }
}
