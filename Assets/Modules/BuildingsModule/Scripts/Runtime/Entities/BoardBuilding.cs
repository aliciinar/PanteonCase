using DG.Tweening;
using FlowIoC.PoolModule.Entities;
using Modules.GridModule.Shared.Entities;
using UnityEngine;

namespace Modules.BuildingsModule.Entities
{
    /// <summary>
    /// A building standing on the board: its BuildingSprite and a health bar along the top of its footprint. Pooled
    /// (group "board_buildings"), so the same few objects are reused as buildings come and go. The building's data -
    /// the grid's BoardBuildingVO - keeps this object as its View, so a command reaches it through any cell the
    /// building covers and does the work on it: placing it, showing a hit. This object only holds what those commands
    /// work on, and puts itself back the way it came when it returns to the pool.
    /// </summary>
    [RequireComponent(typeof(BuildingSprite))]
    public class BoardBuilding : PoolableItem, IOccupantView
    {
        [SerializeField] private BuildingSprite _sprite;

        [Tooltip("The health bar - hidden while the building is unhurt. Authored one world unit wide.")]
        [SerializeField] private Transform _healthBar;

        [Tooltip("The bar's fill, pivoted on its left end: its x scale is the health left.")]
        [SerializeField] private Transform _healthFill;

        public BuildingSprite Sprite => _sprite;
        public Transform HealthBar => _healthBar;
        public Transform HealthFill => _healthFill;

        /// <summary>The hit flash under way.</summary>
        public Tween Flash { get; set; }

        public override void OnReturnToPool()
        {
            Flash?.Kill();
            Flash = null;
            _sprite.Tint(Color.white);
            _sprite.Clear();
            _healthBar.gameObject.SetActive(false);
            _healthFill.localScale = Vector3.one;
        }
    }
}
