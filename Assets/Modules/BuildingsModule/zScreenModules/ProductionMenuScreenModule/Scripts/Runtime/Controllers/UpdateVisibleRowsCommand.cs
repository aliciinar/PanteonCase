using System.Collections.Generic;
using FlowIoC.BaseModule.Controller;
using FlowIoC.BaseModule.Injectable.Attributes;
using FlowIoC.ScreenModule.Service;
using Modules.BuildingsModule.ProductionMenuScreenModule.Data.ValueObjects;
using Modules.BuildingsModule.ProductionMenuScreenModule.Models;
using Modules.BuildingsModule.ProductionMenuScreenModule.Signals;
using Modules.BuildingsModule.ProductionMenuScreenModule.ViewsMediators;
using UnityEngine;

namespace Modules.BuildingsModule.ProductionMenuScreenModule.Controllers
{
    /// <summary>
    /// Decides which rows of the endless menu are on screen for where the scroll stands, and announces
    /// the rows that came into view and the ones that left. Nothing is announced while the scroll stays
    /// within the same rows.
    /// </summary>
    internal class UpdateVisibleRowsCommand : Command
    {
        [Inject]       private IScreenService                      _screenService       { get; set; }
        [Inject]       private IProductionMenuModel                _productionMenuModel { get; set; }
        [InjectSignal] private ProductionMenuScreenInternalSignals _internalSignals     { get; set; }

        public override void Execute()
        {
            if (!_screenService.TryGet.Screen(out ProductionMenuScreenView screen))
            {
                Stop();
                return;
            }

            ScrollStateVO scroll = screen.ScrollState;

            // Row r covers [r × rowHeight, (r + 1) × rowHeight) measured down from the top of row 0.
            int first = Mathf.FloorToInt(scroll.Offset / scroll.RowHeight);
            int last = Mathf.FloorToInt((scroll.Offset + scroll.ViewportHeight) / scroll.RowHeight);

            var entered = new List<int>();
            var left = new List<int>();
            _productionMenuModel.SetVisibleRows(first, last, entered, left);

            if (left.Count > 0) _internalSignals.RowsLeft.Dispatch(left);
            if (entered.Count > 0) _internalSignals.RowsEntered.Dispatch(entered);
        }
    }
}
