using System.Collections.Generic;
using FlowIoC.BaseModule.Controller;
using FlowIoC.BaseModule.Injectable.Attributes;
using FlowIoC.PoolModule.Services;
using FlowIoC.ScreenModule.Service;
using Modules.BuildingsModule.ProductionMenuScreenModule.Entities;
using Modules.BuildingsModule.ProductionMenuScreenModule.ViewsMediators;

namespace Modules.BuildingsModule.ProductionMenuScreenModule.Controllers
{
    /// <summary>Takes the rows that scrolled out of view off the menu and sends their cards back to the pool.</summary>
    internal class ReleaseProductionRowsCommand : Command
    {
        [Inject]      private IScreenService _screenService { get; set; }
        [Inject]      private IPoolService   _poolService   { get; set; }
        [SignalParam] private List<int>      _rows          { get; set; }

        public override void Execute()
        {
            if (!_screenService.TryGet.Screen(out ProductionMenuScreenView screen))
            {
                Stop();
                return;
            }

            foreach (ProductionItem card in screen.RemoveRows(_rows))
                _poolService.Return.Item(card);
        }
    }
}
