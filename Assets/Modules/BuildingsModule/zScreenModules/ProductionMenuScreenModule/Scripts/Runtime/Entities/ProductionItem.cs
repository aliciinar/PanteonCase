using System;
using FlowIoC.PoolModule.Entities;
using Modules.BuildingsModule.Shared.Enums;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Modules.BuildingsModule.ProductionMenuScreenModule.Entities
{
    /// <summary>
    /// One fixed-size card of the production menu. Pooled: the menu recycles the same few cards and
    /// rebinds them to whichever building scrolls into their slot.
    /// </summary>
    public class ProductionItem : PoolableItem
    {
        [SerializeField] private Button _button;
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _label;

        public Action<BuildType> Clicked;

        public RectTransform RectTransform => (RectTransform)transform;

        private BuildType _buildType;

        public override void OnInitialized() => _button.onClick.AddListener(() => Clicked?.Invoke(_buildType));

        public override void OnReturnToPool() => Clicked = null;

        public void Bind(BuildType buildType, Sprite sprite)
        {
            _buildType = buildType;
            _icon.sprite = sprite;
            _label.text = buildType.ToString();
        }
    }
}
