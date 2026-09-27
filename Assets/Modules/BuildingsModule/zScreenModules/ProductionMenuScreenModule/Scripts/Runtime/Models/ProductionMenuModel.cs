using System.Collections.Generic;
using FlowIoC.BaseModule.Adapters;
using FlowIoC.BaseModule.Constructables;
using FlowIoC.BaseModule.Injectable.Attributes;
using Modules.BuildingsModule.ProductionMenuScreenModule.Data.UnityObjects;
using Modules.BuildingsModule.ProductionMenuScreenModule.Data.ValueObjects;
using Modules.BuildingsModule.ProductionMenuScreenModule.RootsContexts;
using Modules.BuildingsModule.Shared.Data.UnityObjects;
using Modules.BuildingsModule.Shared.Data.ValueObjects;
using Modules.BuildingsModule.Shared.Enums;
using UnityEngine;

namespace Modules.BuildingsModule.ProductionMenuScreenModule.Models
{
    /// <summary>
    /// Reads CD_ProductionMenu and CD_Buildings off the Root's adapter in PostConstruct - the screen's context is
    /// listed on BuildingsSystemRoot, so that is the Root - and keeps the menu's entries in CD_Buildings' order, each
    /// with its icon and label, along with the range of rows on screen.
    /// </summary>
    internal class ProductionMenuModel : IProductionMenuModel, IConstructable
    {
        [Inject(nameof(ProductionMenuScreenContext))]
        private GameObject _root { get; set; }

        public bool IsPostConstructed { get; set; }
        public bool IsDeconstructed { get; set; }

        public ProductionGridVO Grid { get; private set; }
        public int FirstVisibleRow { get; private set; }
        public int LastVisibleRow { get; private set; }
        public IReadOnlyList<int> RowsEntered => _rowsEntered;

        private readonly List<ProductionItemVO> _entries = new();
        private readonly List<int> _rowsEntered = new();
        private bool _hasVisibleRows;

        public void PostConstruct()
        {
            var adapter = _root.GetComponent<RootAdapter>();

            var menu = adapter.GetScriptable<CD_ProductionMenu>();
            Grid = new ProductionGridVO(menu.Columns, menu.CellSize, menu.Spacing);

            foreach (KeyValuePair<BuildType, BuildingCVO> building in adapter.GetScriptable<CD_Buildings>().Buildings)
                _entries.Add(new ProductionItemVO(building.Key, building.Value.Icon, building.Key.ToString()));
        }

        public void Deconstruct()
        {
            _entries.Clear();
            ClearVisibleRows();
        }

        public ProductionItemVO EntryAt(int row, int column)
        {
            int index = row * Grid.Columns + column;
            int count = _entries.Count;
            return _entries[(index % count + count) % count];
        }

        public bool SetVisibleRows(int first, int last)
        {
            if (_hasVisibleRows && first == FirstVisibleRow && last == LastVisibleRow) return false;

            _rowsEntered.Clear();
            for (int row = first; row <= last; row++)
                if (!_hasVisibleRows || row < FirstVisibleRow || row > LastVisibleRow) _rowsEntered.Add(row);

            FirstVisibleRow = first;
            LastVisibleRow = last;
            _hasVisibleRows = true;
            return true;
        }

        public void ClearVisibleRows()
        {
            _hasVisibleRows = false;
            _rowsEntered.Clear();
        }
    }
}
