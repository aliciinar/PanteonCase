#if UNITY_EDITOR
using System.Collections;
using Modules.BuildingsModule.Shared.Data.ValueObjects;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace Modules.BuildingsModule.Editor
{
    /// <summary>
    /// Draws BuildingCVO's ExitPoint and SpawnPoint in the Inspector as a small map instead of two bare
    /// numbers each: the building's footprint, a ring of cells around it, the door units come out of (E,
    /// blue, a cell on the building's edge) and the cell they walk to (S, green, outside the building). Each
    /// field's map shows its own point bright and the other one faint; clicking a cell moves the field's
    /// point there, and a sentence under the map says where it is in words. A door off the building's edge
    /// or a spawn point on the building is reported as an error. The numeric field stays above the map for
    /// exact values.
    ///
    /// An Odin drawer because Odin draws CD_Buildings' dictionary itself - its values are Odin properties,
    /// not SerializedProperties a Unity PropertyDrawer would receive.
    /// </summary>
    internal class UnitPointsDrawer : OdinValueDrawer<Vector2Int>
    {
        private const float CellSize = 18f;

        private static readonly Color FootprintColor = new(0.55f, 0.42f, 0.28f);
        private static readonly Color EmptyColor = new(0.3f, 0.3f, 0.3f);
        private static readonly Color SpawnColor = new(0.3f, 0.75f, 0.35f);
        private static readonly Color ExitColor = new(0.3f, 0.55f, 0.9f);
        private static readonly Color InvalidColor = new(0.85f, 0.25f, 0.25f);

        public override bool CanDrawTypeFilter(System.Type type) => type == typeof(Vector2Int);

        protected override bool CanDrawValueProperty(InspectorProperty property) =>
            property.ParentType == typeof(BuildingCVO) &&
            (property.Name == nameof(BuildingCVO.SpawnPoint) || property.Name == nameof(BuildingCVO.ExitPoint));

        private bool IsExit => Property.Name == nameof(BuildingCVO.ExitPoint);

        protected override void DrawPropertyLayout(GUIContent label)
        {
            CallNextDrawer(label);

            var siblings = Property.Parent.Children;
            var size = (Vector2Int)siblings[nameof(BuildingCVO.Size)].ValueEntry.WeakSmartValue;
            var units = (ICollection)siblings[nameof(BuildingCVO.ProducibleUnits)].ValueEntry.WeakSmartValue;

            if (units.Count == 0)
            {
                SirenixEditorGUI.InfoMessageBox($"This building produces no units, so its {(IsExit ? "exit" : "spawn")} point is not used.");
                return;
            }

            string otherName = IsExit ? nameof(BuildingCVO.SpawnPoint) : nameof(BuildingCVO.ExitPoint);
            var other = (Vector2Int)siblings[otherName].ValueEntry.WeakSmartValue;
            Vector2Int point = ValueEntry.SmartValue;
            bool isValid = IsExit ? IsOnEdge(point, size) : !IsInFootprint(point, size);

            DrawMap(size, point, other, isValid);

            if (IsExit)
            {
                if (isValid)
                    SirenixEditorGUI.InfoMessageBox($"Units come out of the building on {DescribeDoor(point, size)}. Click a cell on the building's edge to move the exit point (E).");
                else
                    SirenixEditorGUI.ErrorMessageBox("The exit point is the building's door: it must be a cell on the building's edge. Click one.");
            }
            else
            {
                if (isValid)
                    SirenixEditorGUI.InfoMessageBox($"Units walk to {Describe(point, size)} once they come out - or, when it is taken, to the free cell nearest it. Click a cell to move the spawn point (S).");
                else
                    SirenixEditorGUI.ErrorMessageBox("The spawn point is on the building: units would walk onto it. Click a cell outside it.");
            }
        }

        /// <summary>
        /// The footprint with one ring of cells around it, grown to reach both points. Rows are drawn
        /// top-down, so the building's bottom row sits at the bottom of the map as it does on the board.
        /// </summary>
        private void DrawMap(Vector2Int size, Vector2Int point, Vector2Int other, bool isValid)
        {
            int minX = Mathf.Min(-1, Mathf.Min(point.x, other.x)), minY = Mathf.Min(-1, Mathf.Min(point.y, other.y));
            int maxX = Mathf.Max(size.x, Mathf.Max(point.x, other.x)), maxY = Mathf.Max(size.y, Mathf.Max(point.y, other.y));

            Rect map = GUILayoutUtility.GetRect((maxX - minX + 1) * CellSize, (maxY - minY + 1) * CellSize,
                                                GUILayout.ExpandWidth(false));
            map.x += EditorGUI.indentLevel * 15f;

            Color ownColor = IsExit ? ExitColor : SpawnColor;
            Color otherColor = Faint(IsExit ? SpawnColor : ExitColor);
            string ownMark = IsExit ? "E" : "S";
            string otherMark = IsExit ? "S" : "E";

            for (int y = maxY; y >= minY; y--)
            {
                for (int x = minX; x <= maxX; x++)
                {
                    var cell = new Vector2Int(x, y);
                    var rect = new Rect(map.x + (x - minX) * CellSize, map.y + (maxY - y) * CellSize,
                                        CellSize - 1f, CellSize - 1f);

                    Color color = cell == point ? (isValid ? ownColor : InvalidColor)
                        : cell == other ? otherColor
                        : IsInFootprint(cell, size) ? FootprintColor : EmptyColor;
                    EditorGUI.DrawRect(rect, color);

                    if (cell == point) GUI.Label(rect, ownMark, EditorStyles.centeredGreyMiniLabel);
                    else if (cell == other) GUI.Label(rect, otherMark, EditorStyles.centeredGreyMiniLabel);
                    else if (cell == Vector2Int.zero) GUI.Label(rect, "0,0", EditorStyles.centeredGreyMiniLabel);

                    if (Event.current.type == EventType.MouseDown && rect.Contains(Event.current.mousePosition))
                    {
                        ValueEntry.SmartValue = cell;
                        GUI.changed = true;
                        Event.current.Use();
                    }
                }
            }
        }

        private static Color Faint(Color color) => Color.Lerp(color, EmptyColor, 0.6f);

        private static bool IsInFootprint(Vector2Int cell, Vector2Int size) =>
            cell.x >= 0 && cell.y >= 0 && cell.x < size.x && cell.y < size.y;

        private static bool IsOnEdge(Vector2Int cell, Vector2Int size) =>
            IsInFootprint(cell, size) && (cell.x == 0 || cell.y == 0 || cell.x == size.x - 1 || cell.y == size.y - 1);

        /// <summary>Which edge of the building a door is on, and where along it.</summary>
        private static string DescribeDoor(Vector2Int door, Vector2Int size)
        {
            string vertical = door.y == 0 ? "bottom" : door.y == size.y - 1 ? "top" : null;
            string horizontal = door.x == 0 ? "left" : door.x == size.x - 1 ? "right" : null;

            if (vertical != null && horizontal != null) return $"its {vertical}-{horizontal} corner";
            if (vertical != null) return $"its {vertical} edge, column {door.x + 1} of {size.x} (counted from the left)";
            return $"its {horizontal} edge, row {door.y + 1} of {size.y} (counted from the bottom)";
        }

        /// <summary>Where a point outside the building is, in words: which side, how far, and which cell along it.</summary>
        private static string Describe(Vector2Int point, Vector2Int size)
        {
            int below = -point.y, above = point.y - size.y + 1, left = -point.x, right = point.x - size.x + 1;
            string vertical = below > 0 ? "below" : above > 0 ? "above" : null;
            string horizontal = left > 0 ? "left of" : right > 0 ? "right of" : null;

            if (vertical != null && horizontal != null)
                return $"a cell off the building's {(below > 0 ? "bottom" : "top")}-{(left > 0 ? "left" : "right")} corner";

            if (vertical != null)
                return $"the cell {Cells(below > 0 ? below : above)} {vertical} the building, in front of column {point.x + 1} of {size.x} (counted from the left)";

            return $"the cell {Cells(left > 0 ? left : right)} {horizontal} the building, beside row {point.y + 1} of {size.y} (counted from the bottom)";
        }

        private static string Cells(int distance) => distance == 1 ? "right" : $"{distance} cells";
    }
}
#endif
