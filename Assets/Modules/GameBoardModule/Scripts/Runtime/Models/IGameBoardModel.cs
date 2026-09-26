using System.Collections.Generic;
using Modules.BuildingsModule.Shared.Enums;
using Modules.GameBoardModule.Data.ValueObjects;
using Modules.GameBoardModule.Shared.Data.ValueObjects;
using UnityEngine;

namespace Modules.GameBoardModule.Models
{
    /// <summary>
    /// The board's geometry: its cells and where each one sits in the world. The grid is centred on
    /// the world origin; cell [0, 0] is the bottom-left one.
    /// </summary>
    public interface IGameBoardModel
    {
        /// <summary>Columns (x) and rows (y).</summary>
        Vector2Int GridSize { get; }

        /// <summary>Edge of one cell in world units.</summary>
        float CellSize { get; }

        /// <summary>The world rect the cells cover.</summary>
        Rect GridBounds { get; }

        /// <summary>The grid plus the padding the frame is drawn in.</summary>
        Rect FrameBounds { get; }

        /// <summary>
        /// Every cell, indexed [column, row], each with what stands on it. Held by RD_GameBoard; the
        /// model hands out the asset's array, so what it returns is always what the asset holds.
        /// </summary>
        CellVO[,] Cells { get; }

        /// <summary>Every building's footprint in cells and its sprite, as CD_BoardBuildings authors them.</summary>
        IReadOnlyDictionary<BuildType, BoardBuildingCVO> Buildings { get; }

        /// <summary>The cell a world position falls in. May lie outside the grid - check with IsInside.</summary>
        Vector2Int WorldToCell(Vector3 worldPosition);

        bool IsInside(Vector2Int cell);
    }
}
