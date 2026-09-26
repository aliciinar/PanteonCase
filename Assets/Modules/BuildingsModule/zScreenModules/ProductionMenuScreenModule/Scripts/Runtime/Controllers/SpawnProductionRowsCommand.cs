using System.Collections.Generic;
using FlowIoC.BaseModule.Controller;
using FlowIoC.BaseModule.Injectable.Attributes;
using FlowIoC.PoolModule.Services;
using FlowIoC.ScreenModule.Service;
using Modules.BuildingsModule.ProductionMenuScreenModule.Data.ValueObjects;
using Modules.BuildingsModule.ProductionMenuScreenModule.Entities;
using Modules.BuildingsModule.ProductionMenuScreenModule.Models;
using Modules.BuildingsModule.ProductionMenuScreenModule.ViewsMediators;

namespace Modules.BuildingsModule.ProductionMenuScreenModule.Controllers
{
    /// <summary>
    /// Fills the rows that scrolled into view: the model says which building each slot shows, the cards
    /// come out of the pool (warmed while the game loaded, group "buildings"), and the menu places them.
    /// </summary>
    internal class SpawnProductionRowsCommand : Command
    {
        /// <summary>The key of the card in CD_PoolGroup_Buildings.</summary>
        private const string ItemPoolKey = "production_item";

        [Inject]      private IScreenService       _screenService       { get; set; }
        [Inject]      private IProductionMenuModel _productionMenuModel { get; set; }
        [Inject]      private IPoolService         _poolService         { get; set; }
        [SignalParam] private List<int>            _rows                { get; set; }

        public override void Execute()
        {
            if (!_screenService.TryGet.Screen(out ProductionMenuScreenView screen))
            {
                Stop();
                return;
            }

            int columns = _productionMenuModel.Columns;
            var spawned = new List<ProductionRowVO>(_rows.Count);

            foreach (int row in _rows)
            {
                var cards = new List<ProductionItem>(columns);
                var items = new ProductionItemVO[columns];

                for (int column = 0; column < columns; column++)
                {
                    items[column] = _productionMenuModel.ItemAt(row, column);
                    cards.Add(_poolService.Get<ProductionItem>(ItemPoolKey, null));
                }

                spawned.Add(new ProductionRowVO(row, cards, items));
            }

            screen.PlaceRows(spawned);
        }
    }
}
