using Modules.BuildingsModule.ProductionMenuScreenModule.Data.ValueObjects;

namespace Modules.BuildingsModule.ProductionMenuScreenModule.Models
{
    /// <summary>
    /// The production menu's content, its grid and which of its rows are on screen. The content loops, so every row
    /// of the endless grid - however far the player scrolls - has something to show.
    /// </summary>
    internal interface IProductionMenuModel
    {
        /// <summary>The menu's grid, not yet centred: the view centres it across its content.</summary>
        ProductionGridVO Grid { get; }

        /// <summary>Whether rows <paramref name="first"/> to <paramref name="last"/> are exactly the rows on screen.</summary>
        bool AreVisibleRows(int first, int last);

        /// <summary>Whether this row is on screen. None is while the menu is closed.</summary>
        bool IsRowVisible(int row);

        /// <summary>What the card at this row and column shows. Any row, negative included.</summary>
        ProductionItemVO EntryAt(int row, int column);

        /// <summary>Rows <paramref name="first"/> to <paramref name="last"/> are the ones on screen now.</summary>
        void SetVisibleRows(int first, int last);

        /// <summary>No row is visible any more - the menu closed.</summary>
        void ClearVisibleRows();
    }
}
