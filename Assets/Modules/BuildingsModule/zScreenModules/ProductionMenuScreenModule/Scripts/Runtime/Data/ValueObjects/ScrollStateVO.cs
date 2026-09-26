namespace Modules.BuildingsModule.ProductionMenuScreenModule.Data.ValueObjects
{
    /// <summary>Where the menu's scroll stands, as the view measures it, in canvas units.</summary>
    internal readonly struct ScrollStateVO
    {
        /// <summary>How far the content has scrolled down from row 0 (negative above it).</summary>
        public readonly float Offset;

        /// <summary>Height of the visible part of the menu.</summary>
        public readonly float ViewportHeight;

        /// <summary>Height of one row, card plus spacing.</summary>
        public readonly float RowHeight;

        public ScrollStateVO(float offset, float viewportHeight, float rowHeight)
        {
            Offset = offset;
            ViewportHeight = viewportHeight;
            RowHeight = rowHeight;
        }
    }
}
