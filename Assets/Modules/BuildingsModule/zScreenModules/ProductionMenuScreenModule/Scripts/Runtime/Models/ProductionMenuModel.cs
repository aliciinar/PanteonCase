using System.Collections.Generic;
using FlowIoC.BaseModule.Adapters;
using FlowIoC.BaseModule.Constructables;
using FlowIoC.BaseModule.Injectable.Attributes;
using FlowIoC.BaseModule.SharedData;
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
    /// Reads CD_ProductionMenu off the Root's adapter in PostConstruct - the screen's context is listed on
    /// BuildingsSystemRoot, so that is the Root - and CD_Buildings through ISharedDataModel, where that Root files it
    /// for every module that shows buildings. Keeps the menu's entries in CD_Buildings' order, each with its icon and
    /// label, along with the range of rows on screen.
    /// </summary>
    internal class ProductionMenuModel : IProductionMenuModel, IConstructable
    {
        [Inject(nameof(ProductionMenuScreenContext))]
        private GameObject _root { get; set; }

        [Inject] private ISharedDataModel _sharedDataModel { get; set; }

        public bool IsPostConstructed { get; set; }
        public bool IsDeconstructed { get; set; }

        public ProductionGridVO Grid { get; private set; }
        private readonly List<ProductionItemVO> _entries = new();
        private int _firstVisibleRow;
        private int _lastVisibleRow;
        private bool _hasVisibleRows;

        public void PostConstruct()
        {
            var menu = _root.GetComponent<RootAdapter>().GetScriptable<CD_ProductionMenu>();
            Grid = new ProductionGridVO(menu.Columns, menu.CellSize, menu.Spacing);

            foreach (KeyValuePair<BuildType, BuildingCVO> building in _sharedDataModel.GetScriptable<CD_Buildings>().Buildings)
                _entries.Add(new ProductionItemVO(building.Key, building.Value.Icon, building.Value.Name));
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

        public bool AreVisibleRows(int first, int last) =>
            _hasVisibleRows && first == _firstVisibleRow && last == _lastVisibleRow;

        public bool IsRowVisible(int row) => _hasVisibleRows && row >= _firstVisibleRow && row <= _lastVisibleRow;

        public void SetVisibleRows(int first, int last)
        {
            _firstVisibleRow = first;
            _lastVisibleRow = last;
            _hasVisibleRows = true;
        }

        public void ClearVisibleRows() => _hasVisibleRows = false;
    }
}
