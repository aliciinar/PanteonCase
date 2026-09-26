using System.Collections.Generic;
using Modules.BuildingsModule.ProductionMenuScreenModule.Data.ValueObjects;

namespace Modules.BuildingsModule.ProductionMenuScreenModule.Models
{
    /// <summary>
    /// The production menu's content and which of its rows are on screen. The list loops, so every row
    /// of the endless grid - however far the player scrolls - has something to show.
    /// </summary>
    internal interface IProductionMenuModel
    {
        /// <summary>Cards per row.</summary>
        int Columns { get; }

        /// <summary>What the card at this row and column shows. Any row, negative included.</summary>
        ProductionItemVO ItemAt(int row, int column);

        /// <summary>
        /// Makes rows <paramref name="first"/> to <paramref name="last"/> the visible ones and fills
        /// <paramref name="entered"/> and <paramref name="left"/> with what changed.
        /// </summary>
        void SetVisibleRows(int first, int last, List<int> entered, List<int> left);

        /// <summary>No row is visible any more - the menu closed.</summary>
        void ClearVisibleRows();
    }
}
