using System.Collections.Generic;
using Modules.BuildingsModule.ProductionMenuScreenModule.Entities;

namespace Modules.BuildingsModule.ProductionMenuScreenModule.Data.ValueObjects
{
    /// <summary>A row of the menu ready to be placed: the cards taken from the pool and what each shows.</summary>
    internal class ProductionRowVO
    {
        /// <summary>Row index in the endless list; the view positions the row by it.</summary>
        public int Row { get; }

        /// <summary>One card per column, left to right.</summary>
        public List<ProductionItem> Cards { get; }

        /// <summary>What each card shows, matching Cards by index.</summary>
        public ProductionItemVO[] Items { get; }

        public ProductionRowVO(int row, List<ProductionItem> cards, ProductionItemVO[] items)
        {
            Row = row;
            Cards = cards;
            Items = items;
        }
    }
}
