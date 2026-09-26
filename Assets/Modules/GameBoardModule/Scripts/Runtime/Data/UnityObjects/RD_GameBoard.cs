using System;
using Modules.GameBoardModule.Shared.Data.ValueObjects;
using Modules.GameBoardModule.Shared.Enums;
using Sirenix.OdinInspector;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Modules.GameBoardModule.Data.UnityObjects
{
    /// <summary>
    /// The board as it stands during play: every cell and what occupies it. Filed on GameBoardSystemRoot's
    /// RootAdapter; GameBoardModel keeps this asset, not a copy of its cells, so whoever reads the model
    /// always sees what the asset holds.
    ///
    /// Select the asset while playing to watch the board in the Inspector. The cells are never serialized:
    /// they are rebuilt every session and nothing of a session is written into the asset file.
    /// </summary>
    [CreateAssetMenu(fileName = "RD_GameBoard", menuName = "Game/Data/RD_GameBoard")]
    internal class RD_GameBoard : ScriptableObject
    {
        [NonSerialized]
        [ShowInInspector, ReadOnly]
        [InfoBox("Filled in Play mode. Columns left to right; the table's top row is the board's bottom row (row 0).")]
        [TableMatrix(DrawElementMethod = nameof(DrawCell), SquareCells = true, IsReadOnly = true)]
        public CellVO[,] Cells;

#if UNITY_EDITOR
        private static readonly Color FreeColor = new(0.22f, 0.24f, 0.27f);
        private static readonly Color BuildingColor = new(0.78f, 0.35f, 0.25f);
        private static readonly Color SoldierColor = new(0.25f, 0.5f, 0.85f);
#endif

        /// <summary>
        /// Odin draws each cell of the table through this: free cells grey, buildings red, soldiers blue,
        /// labelled with the occupant's entity id. The method exists in builds too, empty, so the
        /// attribute's nameof still compiles there.
        /// </summary>
        private static CellVO DrawCell(Rect rect, CellVO cell)
        {
#if UNITY_EDITOR
            var inner = new Rect(rect.x + 1f, rect.y + 1f, rect.width - 2f, rect.height - 2f);

            if (cell.IsFree)
            {
                EditorGUI.DrawRect(inner, FreeColor);
                return cell;
            }

            Color color = cell.Occupant.Type == CellOccupantType.Building ? BuildingColor : SoldierColor;
            EditorGUI.DrawRect(inner, color);
            GUI.Label(rect, cell.Occupant.EntityId.ToString(), EditorStyles.centeredGreyMiniLabel);
#endif
            return cell;
        }
    }
}
