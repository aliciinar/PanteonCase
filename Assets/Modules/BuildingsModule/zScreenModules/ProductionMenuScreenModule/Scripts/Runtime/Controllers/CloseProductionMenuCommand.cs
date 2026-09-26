using System.Collections.Generic;
using FlowIoC.BaseModule.Controller;
using FlowIoC.BaseModule.Injectable.Attributes;
using FlowIoC.PoolModule.Services;
using Modules.BuildingsModule.ProductionMenuScreenModule.Entities;
using Modules.BuildingsModule.ProductionMenuScreenModule.Models;

namespace Modules.BuildingsModule.ProductionMenuScreenModule.Controllers
{
    /// <summary>
    /// The menu closed: every card it held goes back to the pool and no row counts as visible, so the
    /// next opening starts from scratch.
    /// </summary>
    internal class CloseProductionMenuCommand : Command
    {
        [Inject]      private IPoolService         _poolService         { get; set; }
        [Inject]      private IProductionMenuModel _productionMenuModel { get; set; }
        [SignalParam] private List<ProductionItem> _cards               { get; set; }

        public override void Execute()
        {
            foreach (ProductionItem card in _cards)
                _poolService.Return.Item(card);

            _productionMenuModel.ClearVisibleRows();
        }
    }
}
