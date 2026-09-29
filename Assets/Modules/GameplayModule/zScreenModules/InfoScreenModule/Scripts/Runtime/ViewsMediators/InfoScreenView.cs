using System;
using System.Collections.Generic;
using FlowIoC.BaseModule.Injectable.Components;
using FlowIoC.ScreenModule.ViewsMediators.Screen;
using Modules.GameplayModule.InfoScreenModule.Data.ValueObjects;
using Modules.GameplayModule.InfoScreenModule.Entities;
using Modules.UnitsModule.Shared.Data.ValueObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Modules.GameplayModule.InfoScreenModule.ViewsMediators
{
    /// <summary>
    /// The selected building or unit, drawn inside the information panel under its header: its image, name and
    /// health; for a unit its damage, for a building that produces units a card per unit, laid out by the grid
    /// under the Production title. The section is hidden when there are no cards. What it shows and where the
    /// cards come from is decided by the commands.
    /// </summary>
    [RequireComponent(typeof(ViewInjector))]
    public class InfoScreenView : ScreenView
    {
        [SerializeField] private Image _image;
        [SerializeField] private TMP_Text _name;
        [SerializeField] private TMP_Text _hp;
        [SerializeField] private TMP_Text _damage;
        [SerializeField] private GameObject _production;
        [SerializeField] private RectTransform _units;

        public Action<UnitSpawnRequestVO> UnitClicked;

        private readonly List<UnitItem> _cards = new();

        internal void ShowBuilding(string buildingName, Sprite image, int hp, int maxHp, IReadOnlyList<UnitCardVO> units)
        {
            _image.sprite = image;
            _name.text = buildingName;
            _hp.text = $"HP  {hp} / {maxHp}";
            _damage.gameObject.SetActive(false);

            foreach (UnitCardVO unit in units)
            {
                unit.Card.Bind(unit.Request, unit.Sprite, unit.Name);
                unit.Card.Clicked += OnUnitClicked;
                unit.Card.RectTransform.SetParent(_units, false);
                _cards.Add(unit.Card);
            }

            _production.SetActive(units.Count > 0);
        }

        /// <summary>Shows a unit; it produces nothing, so the production section is hidden. Expects no cards on the screen.</summary>
        internal void ShowUnit(string unitName, Sprite image, int hp, int maxHp, int damage)
        {
            _image.sprite = image;
            _name.text = unitName;
            _hp.text = $"HP  {hp} / {maxHp}";
            _damage.text = $"Damage  {damage}";
            _damage.gameObject.SetActive(true);
            _production.SetActive(false);
        }

        /// <summary>Takes every unit card off the screen and hands them back.</summary>
        public List<UnitItem> RemoveUnits()
        {
            var cards = new List<UnitItem>(_cards);
            foreach (UnitItem card in cards) card.Clicked -= OnUnitClicked;
            _cards.Clear();
            return cards;
        }

        private void OnUnitClicked(UnitSpawnRequestVO request) => UnitClicked?.Invoke(request);
    }
}
