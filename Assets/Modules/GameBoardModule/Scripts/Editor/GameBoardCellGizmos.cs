#if UNITY_EDITOR
using Modules.GameBoardModule.Shared.Data.ValueObjects;
using Modules.GameBoardModule.ViewsMediators;
using UnityEditor;
using UnityEngine;

namespace Modules.GameBoardModule.Editor
{
    /// <summary>
    /// Draws a one-cell square (CellPixelSize px) around every cell of the board, so the cell
    /// positions the model computed can be checked in the Scene view. Cells exist once the board
    /// has been built, i.e. in Play mode.
    /// </summary>
    internal static class GameBoardCellGizmos
    {
        private static readonly Color CellColor = new(0.3f, 0.85f, 1f, 0.8f);

        [DrawGizmo(GizmoType.NonSelected | GizmoType.Selected)]
        private static void DrawCells(GameBoardView view, GizmoType gizmoType)
        {
            Gizmos.color = CellColor;
            var cellSize = new Vector3(view.CellSize, view.CellSize, 0f);

            foreach (CellVO cell in view.Cells)
                Gizmos.DrawWireCube(cell.Position, cellSize);
        }
    }
}
#endif
