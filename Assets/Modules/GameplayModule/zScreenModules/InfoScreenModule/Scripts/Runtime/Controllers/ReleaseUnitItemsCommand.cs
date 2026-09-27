using System.Collections.Generic;
using FlowIoC.BaseModule.Controller;
using FlowIoC.BaseModule.Injectable.Attributes;
using FlowIoC.PoolModule.Services;
using Modules.GameplayModule.InfoScreenModule.Entities;

namespace Modules.GameplayModule.InfoScreenModule.Controllers
{
    /// <summary>The screen closed: every unit card it held goes back to the pool.</summary>
    internal class ReleaseUnitItemsCommand : Command
    {
        [Inject]      private IPoolService   _poolService { get; set; }
        [SignalParam] private List<UnitItem> _cards       { get; set; }

        public override void Execute()
        {
            foreach (UnitItem card in _cards)
                _poolService.Return.Item(card);
        }
    }
}
