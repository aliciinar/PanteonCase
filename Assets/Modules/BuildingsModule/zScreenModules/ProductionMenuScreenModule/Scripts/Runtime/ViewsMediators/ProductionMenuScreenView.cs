using System;
using System.Collections.Generic;
using FlowIoC.BaseModule.Injectable.Components;
using FlowIoC.ScreenModule.ViewsMediators.Screen;
using Modules.BuildingsModule.ProductionMenuScreenModule.Data.ValueObjects;
using Modules.BuildingsModule.ProductionMenuScreenModule.Entities;
using Modules.BuildingsModule.Shared.Enums;
using UnityEngine;
using UnityEngine.UI;

namespace Modules.BuildingsModule.ProductionMenuScreenModule.ViewsMediators
{
    /// <summary>
    /// The production menu panel on the left of the screen. The ScrollRect scrolls it (Unrestricted, so
    /// it has no ends); this view reports where the scroll stands, places the rows of cards it is handed
    /// at their fixed place in the content, and takes rows off when told. Which rows are on screen, what
    /// they show and where their cards come from is decided by the commands.
    /// </summary>
    [RequireComponent(typeof(ViewInjector))]
    public class ProductionMenuScreenView : ScreenView
    {
        [SerializeField] private ScrollRect _scrollRect;
        [SerializeField] private RectTransform _panel;
        [SerializeField] private Vector2 _cellSize = new(140f, 140f);
        [SerializeField] private Vector2 _spacing = new(16f, 16f);

        /// <summary>The scroll moved.</summary>
        public Action Scrolled;

        public Action<BuildType> ItemClicked;

        private readonly Dictionary<int, List<ProductionItem>> _rows = new();
        private readonly Vector3[] _corners = new Vector3[4];

        private RectTransform Content => _scrollRect.content;
        private float RowHeight => _cellSize.y + _spacing.y;

        private void OnEnable() => _scrollRect.onValueChanged.AddListener(OnScrollValueChanged);

        private void OnDisable() => _scrollRect.onValueChanged.RemoveListener(OnScrollValueChanged);

        /// <summary>Where the scroll stands now, in canvas units.</summary>
        internal ScrollStateVO ScrollState =>
            new(Content.anchoredPosition.y, _scrollRect.viewport.rect.height, RowHeight);

        internal void PlaceRows(IReadOnlyList<ProductionRowVO> rows)
        {
            foreach (ProductionRowVO row in rows)
            {
                for (int i = 0; i < row.Cards.Count; i++)
                {
                    ProductionItem card = row.Cards[i];
                    card.Bind(row.Items[i].Type, row.Items[i].Sprite);
                    card.Clicked += OnItemClicked;

                    RectTransform cell = card.RectTransform;
                    cell.SetParent(Content, false);
                    cell.anchorMin = cell.anchorMax = new Vector2(0f, 1f);
                    cell.pivot = new Vector2(0f, 1f);
                    cell.sizeDelta = _cellSize;
                }

                _rows[row.Row] = row.Cards;
                PositionRow(row.Row, row.Cards);
            }
        }

        /// <summary>Takes these rows off the menu and hands their cards back.</summary>
        public List<ProductionItem> RemoveRows(IReadOnlyList<int> rows)
        {
            var cards = new List<ProductionItem>();

            foreach (int row in rows)
            {
                if (!_rows.Remove(row, out List<ProductionItem> rowCards)) continue;
                cards.AddRange(Detach(rowCards));
            }

            return cards;
        }

        /// <summary>Takes every card off the menu, hands them back, and scrolls back to the top.</summary>
        public List<ProductionItem> RemoveAllRows()
        {
            var cards = new List<ProductionItem>();
            foreach (List<ProductionItem> row in _rows.Values) cards.AddRange(Detach(row));
            _rows.Clear();

            _scrollRect.StopMovement();
            Content.anchoredPosition = Vector2.zero;
            return cards;
        }

        /// <summary>Puts every row back in place - the columns move when the panel's width changes.</summary>
        public void RepositionRows()
        {
            foreach (KeyValuePair<int, List<ProductionItem>> row in _rows)
                PositionRow(row.Key, row.Value);
        }

        /// <summary>The panel's area in normalised screen coordinates (0-1, origin bottom-left).</summary>
        public Rect MeasureArea()
        {
            Canvas canvas = _panel.GetComponentInParent<Canvas>().rootCanvas;
            Camera canvasCamera = canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera;

            _panel.GetWorldCorners(_corners);
            Vector2 bottomLeft = RectTransformUtility.WorldToScreenPoint(canvasCamera, _corners[0]);
            Vector2 topRight = RectTransformUtility.WorldToScreenPoint(canvasCamera, _corners[2]);

            return Rect.MinMaxRect(bottomLeft.x / Screen.width, bottomLeft.y / Screen.height,
                                   topRight.x / Screen.width, topRight.y / Screen.height);
        }

        // Rows sit at fixed places in the content (row 0 at the top, negative rows above it), so
        // scrolling moves the content alone and no card is repositioned while the list moves.
        private void PositionRow(int row, List<ProductionItem> cards)
        {
            float rowWidth = cards.Count * _cellSize.x + (cards.Count - 1) * _spacing.x;
            float left = (Content.rect.width - rowWidth) * 0.5f;
            float y = -row * RowHeight - _spacing.y * 0.5f;

            for (int column = 0; column < cards.Count; column++)
                cards[column].RectTransform.anchoredPosition = new Vector2(left + column * (_cellSize.x + _spacing.x), y);
        }

        private IEnumerable<ProductionItem> Detach(List<ProductionItem> cards)
        {
            foreach (ProductionItem card in cards) card.Clicked -= OnItemClicked;
            return cards;
        }

        private void OnScrollValueChanged(Vector2 normalizedPosition) => Scrolled?.Invoke();

        private void OnItemClicked(BuildType buildType) => ItemClicked?.Invoke(buildType);

        /// <summary>
        /// This method runs if screenData.HasShowAnimation bool is true.
        /// If you don't use custom animations delete this method.
        /// </summary>
        protected override void PlayShowAnimation()
        {
            ShowCompleted?.Invoke(this);
        }

        /// <summary>
        /// This method runs if screenData.HasHideAnimation bool is true.
        /// If you don't use custom animations delete this method.
        /// </summary>
        protected override void PlayHideAnimation()
        {
            HideCompleted?.Invoke(this);
        }
    }
}
