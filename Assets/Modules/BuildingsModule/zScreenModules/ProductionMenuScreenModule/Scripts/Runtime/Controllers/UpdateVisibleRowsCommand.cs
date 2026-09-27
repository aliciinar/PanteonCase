using System.Collections.Generic;
using FlowIoC.BaseModule.Controller;
using FlowIoC.BaseModule.Injectable.Attributes;
using FlowIoC.ScreenModule.Service;
using Modules.BuildingsModule.ProductionMenuScreenModule.Data.ValueObjects;
using Modules.BuildingsModule.ProductionMenuScreenModule.Models;
using Modules.BuildingsModule.ProductionMenuScreenModule.ViewsMediators;
using UnityEngine;

namespace Modules.BuildingsModule.ProductionMenuScreenModule.Controllers
{
    /// <summary>
    /// Works out which rows of the endless menu are on screen for where the scroll stands. While they are the rows
    /// already shown - most of a scroll - the sequence stops here; when they changed, they become the visible rows and
    /// are handed to the next step, which moves the cards, together with the rows that came into view.
    /// </summary>
    internal class UpdateVisibleRowsCommand : Command
    {
        [Inject] private IScreenService       _screenService       { get; set; }
        [Inject] private IProductionMenuModel _productionMenuModel { get; set; }

        public override void Execute()
        {
            Retain();

            if (!_screenService.TryGet.Screen(out ProductionMenuScreenView screen))
            {
                Stop();
                return;
            }

            ScrollStateVO scroll = screen.ScrollState;
            float rowHeight = _productionMenuModel.Grid.RowHeight;

            // Row r covers [r × rowHeight, (r + 1) × rowHeight) measured down from the top of row 0. Floor, not a
            // cast: a row half shown above row 0 is -1.3 → -2, where a cast would drop it.
            int first = Mathf.FloorToInt(scroll.Offset / rowHeight);
            int last = Mathf.FloorToInt((scroll.Offset + scroll.ViewportHeight) / rowHeight);

            if (_productionMenuModel.AreVisibleRows(first, last))
            {
                Stop();
                return;
            }

            var entered = new List<int>();
            for (int row = first; row <= last; row++)
                if (!_productionMenuModel.IsRowVisible(row)) entered.Add(row);

            _productionMenuModel.SetVisibleRows(first, last);
            Release(new VisibleRowsVO(first, last, entered));
        }
    }
}
