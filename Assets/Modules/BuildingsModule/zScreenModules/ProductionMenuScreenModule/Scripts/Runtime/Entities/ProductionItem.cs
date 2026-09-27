using System;
using FlowIoC.PoolModule.Entities;
using Modules.BuildingsModule.ProductionMenuScreenModule.Data.ValueObjects;
using Modules.BuildingsModule.Shared.Enums;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Modules.BuildingsModule.ProductionMenuScreenModule.Entities
{
    /// <summary>
    /// One card of the production menu. Pooled, and recycled in place: while the menu is open a card stays in its
    /// content, and one whose row scrolled out of view is freed and shown in a row that scrolled in - it binds the
    /// building and moves itself to that cell.
    /// </summary>
    public class ProductionItem : PoolableItem
    {
        private const int NoRow = int.MinValue;

        [SerializeField] private Button _button;
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _label;

        public Action<BuildType> Clicked;

        public RectTransform RectTransform => (RectTransform)transform;

        /// <summary>The row the card shows.</summary>
        public int Row { get; private set; } = NoRow;

        /// <summary>On the menu but showing no row, waiting to be shown in one.</summary>
        public bool IsSpare => Row == NoRow;

        private int _column;
        private BuildType _buildType;

        // Anchored and pivoted at the top-left corner, the point the menu's grid positions a card by. Nothing moves
        // either afterwards - the pool included - so it is set once.
        public override void OnInitialized()
        {
            _button.onClick.AddListener(() => Clicked?.Invoke(_buildType));
            RectTransform.anchorMin = RectTransform.anchorMax = RectTransform.pivot = new Vector2(0f, 1f);
        }

        // The pool parks a card keeping its world scale, so it comes back carrying the canvas scale factor it left
        // with; unreset, every trip through the pool shrinks it again.
        public override void OnGetFromPool() => transform.localScale = Vector3.one;

        public override void OnReturnToPool()
        {
            Clicked = null;
            Row = NoRow;
        }

        public void FitToCell(Vector2 size) => RectTransform.sizeDelta = size;

        /// <summary>Shows this entry at this cell of the grid.</summary>
        internal void ShowSlot(int row, int column, ProductionItemVO entry, in ProductionGridVO grid)
        {
            Row = row;
            _column = column;

            _buildType = entry.Type;
            _icon.sprite = entry.Sprite;
            _label.text = entry.Label;

            Reposition(grid);
        }

        /// <summary>Moves back to its cell of the grid - the grid was centred again.</summary>
        internal void Reposition(in ProductionGridVO grid) => RectTransform.anchoredPosition = grid.CellPosition(Row, _column);

        /// <summary>Leaves its row and stays on the menu, spare.</summary>
        public void Free() => Row = NoRow;
    }
}
