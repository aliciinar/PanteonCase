using System;
using System.Collections.Generic;
using FlowIoC.BaseModule.Injectable.Components;
using FlowIoC.ScreenModule.ViewsMediators.Screen;
using Modules.BuildingsModule.BuildingInfoScreenModule.Data.ValueObjects;
using Modules.BuildingsModule.BuildingInfoScreenModule.Entities;
using Modules.UnitsModule.Shared.Enums;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Modules.BuildingsModule.BuildingInfoScreenModule.ViewsMediators
{
    /// <summary>
    /// The selected building, drawn inside the information panel under its header: its image, name and
    /// health, and - for a building that produces units - a card per unit, laid out by the grid under
    /// the Production title. The section is hidden when there are no cards. What it shows and where the
    /// cards come from is decided by the commands.
    /// </summary>
    [RequireComponent(typeof(ViewInjector))]
    public class BuildingInfoScreenView : ScreenView
    {
        [SerializeField] private Image _image;
        [SerializeField] private TMP_Text _name;
        [SerializeField] private TMP_Text _hp;
        [SerializeField] private GameObject _production;
        [SerializeField] private RectTransform _units;

        public Action<UnitType> UnitClicked;

        private readonly List<UnitItem> _cards = new();

        internal void ShowBuilding(string buildingName, Sprite image, int hp, int maxHp, IReadOnlyList<UnitCardVO> units)
        {
            _image.sprite = image;
            _name.text = buildingName;
            _hp.text = $"HP  {hp} / {maxHp}";

            foreach (UnitCardVO unit in units)
            {
                unit.Card.Bind(unit.Type, unit.Sprite);
                unit.Card.Clicked += OnUnitClicked;
                // The pool parks a card keeping its world scale, so it comes back carrying the canvas
                // scale factor it left with; unreset, every trip through the pool shrinks it again.
                unit.Card.RectTransform.SetParent(_units, false);
                unit.Card.RectTransform.localScale = Vector3.one;
                _cards.Add(unit.Card);
            }

            _production.SetActive(units.Count > 0);
        }

        /// <summary>Takes every unit card off the screen and hands them back.</summary>
        public List<UnitItem> RemoveUnits()
        {
            var cards = new List<UnitItem>(_cards);
            foreach (UnitItem card in cards) card.Clicked -= OnUnitClicked;
            _cards.Clear();
            return cards;
        }

        private void OnUnitClicked(UnitType unitType) => UnitClicked?.Invoke(unitType);

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
