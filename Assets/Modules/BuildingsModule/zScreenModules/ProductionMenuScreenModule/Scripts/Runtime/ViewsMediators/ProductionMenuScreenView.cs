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
    /// The production menu panel on the left of the screen. The ScrollRect scrolls it (Unrestricted, so it has no
    /// ends); this view reports where the scroll stands and moves the cards it holds as the commands say. A card
    /// stays in the content for as long as the menu is open: one whose row left the view is freed, spare, and shown
    /// in a row that came in. Which rows are on screen, what they show and when the pool is asked is the commands'.
    /// </summary>
    [RequireComponent(typeof(ViewInjector))]
    public class ProductionMenuScreenView : ScreenView
    {
        [SerializeField] private ScrollRect _scrollRect;
        [SerializeField] private RectTransform _panel;

        /// <summary>The scroll moved.</summary>
        public Action Scrolled;

        public Action<BuildType> ItemClicked;

        /// <summary>Every card on the menu, showing a row or spare.</summary>
        private readonly List<ProductionItem> _cards = new();

        /// <summary>The cards showing no row.</summary>
        private readonly Stack<ProductionItem> _spares = new();

        private readonly Vector3[] _corners = new Vector3[4];

        /// <summary>The grid, centred across the content.</summary>
        private ProductionGridVO _grid;

        private RectTransform Content => _scrollRect.content;

        /// <summary>Where the scroll stands now, in canvas units.</summary>
        internal ScrollStateVO ScrollState => new(Content.anchoredPosition.y, _scrollRect.viewport.rect.height);

        public int SpareCount => _spares.Count;

        private void OnEnable() => _scrollRect.onValueChanged.AddListener(OnScrollValueChanged);

        private void OnDisable() => _scrollRect.onValueChanged.RemoveListener(OnScrollValueChanged);

        // The screen is pooled: every opening starts at the top, standing still.
        public override void BeforeScreenActivation()
        {
            base.BeforeScreenActivation();
            _scrollRect.StopMovement();
            Content.anchoredPosition = Vector2.zero;
        }

        /// <summary>The grid the cards are laid on.</summary>
        internal void SetGrid(ProductionGridVO grid) => _grid = grid.WithContentWidth(Content.rect.width);

        /// <summary>Centres the grid across the content again - its width follows the panel's - and puts every shown card back in its cell.</summary>
        public void Relayout()
        {
            _grid = _grid.WithContentWidth(Content.rect.width);

            foreach (ProductionItem card in _cards)
                if (!card.IsSpare) card.Reposition(_grid);
        }

        /// <summary>Frees every card showing a row outside <paramref name="first"/> to <paramref name="last"/>.</summary>
        public void FreeRowsOutside(int first, int last)
        {
            foreach (ProductionItem card in _cards)
            {
                if (card.IsSpare || (card.Row >= first && card.Row <= last)) continue;

                card.Free();
                _spares.Push(card);
            }
        }

        /// <summary>Takes a card from the pool onto the menu, spare until a row is shown in it.</summary>
        public void AddCard(ProductionItem card)
        {
            card.RectTransform.SetParent(Content, false);
            card.FitToCell(_grid.CellSize);
            card.Clicked += OnItemClicked;

            _cards.Add(card);
            _spares.Push(card);
        }

        /// <summary>Shows this entry at this cell, in a spare card.</summary>
        internal void ShowCell(int row, int column, ProductionItemVO entry) =>
            _spares.Pop().ShowSlot(row, column, entry, _grid);

        /// <summary>Hands back a spare card for the pool - the menu holds more cards than its rows show.</summary>
        public bool TryTakeSpare(out ProductionItem card)
        {
            if (!_spares.TryPop(out card)) return false;

            _cards.Remove(card);
            return true;
        }

        /// <summary>Takes every card off the menu and hands them back - the menu closed.</summary>
        public List<ProductionItem> RemoveAllCards()
        {
            var cards = new List<ProductionItem>(_cards);
            _cards.Clear();
            _spares.Clear();
            return cards;
        }

        /// <summary>
        /// The panel's area in normalised screen coordinates (0-1, origin bottom-left), shrunk to the whole pixels
        /// it paints: at a fractional canvas scale an edge falls mid-pixel, the panel leaves that column unpainted,
        /// and a camera viewport laid beside the raw edge would round away from it and leave a black seam.
        /// </summary>
        public Rect MeasureArea()
        {
            Canvas canvas = _panel.GetComponentInParent<Canvas>().rootCanvas;
            Camera canvasCamera = canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera;

            _panel.GetWorldCorners(_corners);
            Vector2 bottomLeft = RectTransformUtility.WorldToScreenPoint(canvasCamera, _corners[0]);
            Vector2 topRight = RectTransformUtility.WorldToScreenPoint(canvasCamera, _corners[2]);

            return Rect.MinMaxRect(Mathf.Ceil(bottomLeft.x) / Screen.width, Mathf.Ceil(bottomLeft.y) / Screen.height,
                                   Mathf.Floor(topRight.x) / Screen.width, Mathf.Floor(topRight.y) / Screen.height);
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
