using Modules.GridModule.Data.ValueObjects;
using UnityEngine;

namespace Modules.GridModule.Models
{
    /// <summary>The grid's state. The grid service fills it and is the only one that writes it.</summary>
    internal interface IGridModel
    {
        /// <summary>Columns (x) and rows (y).</summary>
        Vector2Int GridSize { get; set; }

        /// <summary>Edge of one cell in world units.</summary>
        float CellSize { get; set; }

        /// <summary>The world rect the cells cover, centred on the world origin.</summary>
        Rect Bounds { get; set; }

        /// <summary>Every cell, indexed [column, row]. Held by RD_Grid; this is the asset's array.</summary>
        CellVO[,] Cells { get; set; }

        /// <summary>
        /// Whether a press on the board is picked - answered with what it landed on. Not while a building is being
        /// placed: then a press only moves that building.
        /// </summary>
        bool IsPicking { get; set; }
    }
}
