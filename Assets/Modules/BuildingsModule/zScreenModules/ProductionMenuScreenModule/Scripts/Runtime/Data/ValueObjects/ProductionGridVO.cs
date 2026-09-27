using UnityEngine;

namespace Modules.BuildingsModule.ProductionMenuScreenModule.Data.ValueObjects
{
    /// <summary>
    /// The production menu's grid, in canvas units: rows of <see cref="Columns"/> cards, row 0 at the top of the
    /// content and negative rows above it. Built from CD_ProductionMenu, then centred across the content once its
    /// width is known.
    /// </summary>
    internal readonly struct ProductionGridVO
    {
        public readonly int Columns;
        public readonly Vector2 CellSize;
        public readonly Vector2 Spacing;

        /// <summary>Left edge of the first column: the content's spare width, split evenly on both sides.</summary>
        private readonly float _left;

        public ProductionGridVO(int columns, Vector2 cellSize, Vector2 spacing) : this(columns, cellSize, spacing, 0f) { }

        private ProductionGridVO(int columns, Vector2 cellSize, Vector2 spacing, float left)
        {
            Columns = columns;
            CellSize = cellSize;
            Spacing = spacing;
            _left = left;
        }

        /// <summary>One row: a card and the gap under it.</summary>
        public float RowHeight => CellSize.y + Spacing.y;

        /// <summary>The same grid, centred across content this wide.</summary>
        public ProductionGridVO WithContentWidth(float contentWidth)
        {
            float rowWidth = Columns * CellSize.x + (Columns - 1) * Spacing.x;
            return new ProductionGridVO(Columns, CellSize, Spacing, (contentWidth - rowWidth) * 0.5f);
        }

        /// <summary>Where the top-left corner of the card at this cell sits in the content.</summary>
        public Vector2 CellPosition(int row, int column) =>
            new(_left + column * (CellSize.x + Spacing.x), -row * RowHeight - Spacing.y * 0.5f);
    }
}
