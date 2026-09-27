using System.Collections.Generic;
using FlowIoC.BaseModule.Controller;
using FlowIoC.BaseModule.Injectable.Attributes;
using FlowIoC.PoolModule.Services;
using FlowIoC.ScreenModule.Service;
using Modules.BuildingsModule.ProductionMenuScreenModule.Entities;
using Modules.BuildingsModule.ProductionMenuScreenModule.Models;
using Modules.BuildingsModule.ProductionMenuScreenModule.ViewsMediators;

namespace Modules.BuildingsModule.ProductionMenuScreenModule.Controllers
{
    /// <summary>
    /// Moves the cards of the rows that left the view to the rows that came in, without the pool: a card stays on
    /// the menu and only shows another building in another cell. The pool is asked only for the difference - more
    /// rows on screen than cards (the first opening, a taller window) - and takes back what fewer rows leave spare.
    /// </summary>
    internal class RecycleProductionRowsCommand : Command
    {
        /// <summary>The key of the card in CD_PoolGroup_Buildings.</summary>
        private const string ItemPoolKey = "production_item";

        [Inject] private IScreenService       _screenService       { get; set; }
        [Inject] private IProductionMenuModel _productionMenuModel { get; set; }
        [Inject] private IPoolService         _poolService         { get; set; }

        public override void Execute()
        {
            Retain();

            if (!_screenService.TryGet.Screen(out ProductionMenuScreenView screen))
            {
                Stop();
                return;
            }

            screen.FreeRowsOutside(_productionMenuModel.FirstVisibleRow, _productionMenuModel.LastVisibleRow);

            IReadOnlyList<int> rowsEntered = _productionMenuModel.RowsEntered;
            int columns = _productionMenuModel.Grid.Columns;

            for (int missing = rowsEntered.Count * columns - screen.SpareCount; missing > 0; missing--)
                screen.AddCard(_poolService.Get<ProductionItem>(ItemPoolKey));

            for (int i = 0; i < rowsEntered.Count; i++)
                for (int column = 0; column < columns; column++)
                    screen.ShowCell(rowsEntered[i], column, _productionMenuModel.EntryAt(rowsEntered[i], column));

            while (screen.TryTakeSpare(out ProductionItem spare))
                _poolService.Return.Item(spare);

            Release();
        }
    }
}
