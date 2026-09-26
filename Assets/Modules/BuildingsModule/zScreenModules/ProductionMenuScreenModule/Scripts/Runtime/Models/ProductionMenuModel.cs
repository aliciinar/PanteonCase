using System.Collections.Generic;
using FlowIoC.BaseModule.Adapters;
using FlowIoC.BaseModule.Constructables;
using FlowIoC.BaseModule.Injectable.Attributes;
using Modules.BuildingsModule.ProductionMenuScreenModule.Data.UnityObjects;
using Modules.BuildingsModule.ProductionMenuScreenModule.Data.ValueObjects;
using Modules.BuildingsModule.ProductionMenuScreenModule.RootsContexts;
using Modules.BuildingsModule.Shared.Enums;
using UnityEngine;

namespace Modules.BuildingsModule.ProductionMenuScreenModule.Models
{
    /// <summary>
    /// Reads CD_ProductionMenu off the Root's adapter in PostConstruct - the screen's context is listed
    /// on BuildingsSystemRoot, so that is the Root - and keeps the menu's entries in the asset's order,
    /// along with the rows currently on screen.
    /// </summary>
    internal class ProductionMenuModel : IProductionMenuModel, IConstructable
    {
        [Inject(nameof(ProductionMenuScreenContext))]
        private GameObject _root { get; set; }

        public bool IsPostConstructed { get; set; }
        public bool IsDeconstructed { get; set; }

        public int Columns { get; private set; }

        private readonly List<ProductionItemVO> _items = new();
        private readonly HashSet<int> _visibleRows = new();

        public void PostConstruct()
        {
            CD_ProductionMenu config = _root.GetComponent<RootAdapter>().GetScriptable<CD_ProductionMenu>();

            Columns = config.Columns;
            foreach (KeyValuePair<BuildType, Sprite> entry in config.Sprites)
                _items.Add(new ProductionItemVO(entry.Key, entry.Value));
        }

        public void Deconstruct()
        {
            _items.Clear();
            _visibleRows.Clear();
        }

        public ProductionItemVO ItemAt(int row, int column)
        {
            int index = row * Columns + column;
            return _items[(index % _items.Count + _items.Count) % _items.Count];
        }

        public void SetVisibleRows(int first, int last, List<int> entered, List<int> left)
        {
            foreach (int row in _visibleRows)
                if (row < first || row > last) left.Add(row);

            foreach (int row in left) _visibleRows.Remove(row);

            for (int row = first; row <= last; row++)
                if (_visibleRows.Add(row)) entered.Add(row);
        }

        public void ClearVisibleRows() => _visibleRows.Clear();
    }
}
