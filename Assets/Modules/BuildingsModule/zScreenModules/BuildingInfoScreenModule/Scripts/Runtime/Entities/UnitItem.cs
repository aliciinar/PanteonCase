using System;
using FlowIoC.PoolModule.Entities;
using Modules.UnitsModule.Shared.Enums;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Modules.BuildingsModule.BuildingInfoScreenModule.Entities
{
    /// <summary>
    /// One unit card of the building info screen: the unit's image and name, clicked to produce one.
    /// Pooled: every building shown takes its cards from the pool and gives them back when it goes.
    /// </summary>
    public class UnitItem : PoolableItem
    {
        [SerializeField] private Button _button;
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _label;

        public Action<UnitType> Clicked;

        public RectTransform RectTransform => (RectTransform)transform;

        private UnitType _unitType;

        public override void OnInitialized() => _button.onClick.AddListener(() => Clicked?.Invoke(_unitType));

        // The pool parks a card keeping its world scale, so it comes back carrying the canvas scale factor
        // it left with; unreset, every trip through the pool shrinks it again.
        public override void OnGetFromPool() => transform.localScale = Vector3.one;

        public override void OnReturnToPool() => Clicked = null;

        public void Bind(UnitType unitType, Sprite sprite)
        {
            _unitType = unitType;
            _icon.sprite = sprite;
            _label.text = unitType.ToString();
        }
    }
}
