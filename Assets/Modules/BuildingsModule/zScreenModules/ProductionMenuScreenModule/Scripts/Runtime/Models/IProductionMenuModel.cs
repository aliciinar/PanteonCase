using System.Collections.Generic;
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

        /// <summary>The first row on screen.</summary>
        int FirstVisibleRow { get; }

        /// <summary>The last row on screen.</summary>
        int LastVisibleRow { get; }

        /// <summary>The rows the last change of the visible rows brought into view.</summary>
        IReadOnlyList<int> RowsEntered { get; }

        /// <summary>What the card at this row and column shows. Any row, negative included.</summary>
        ProductionItemVO EntryAt(int row, int column);

        /// <summary>
        /// Makes rows <paramref name="first"/> to <paramref name="last"/> the visible ones and records the rows that
        /// came into view. False, recording nothing, when they already were the visible ones.
        /// </summary>
        bool SetVisibleRows(int first, int last);

        /// <summary>No row is visible any more - the menu closed.</summary>
        void ClearVisibleRows();
    }
}
