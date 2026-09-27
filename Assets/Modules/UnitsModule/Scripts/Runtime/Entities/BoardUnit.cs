using DG.Tweening;
using FlowIoC.PoolModule.Entities;
using Modules.GridModule.Shared.Entities;
using UnityEngine;

namespace Modules.UnitsModule.Entities
{
    /// <summary>
    /// A unit on the board: one sprite renderer that walks and strikes, and a health bar above it. Pooled (group
    /// "units"), so the same objects are reused as units come and go. The unit's data - the grid's BoardUnitVO - keeps
    /// this object as its View, so a command reaches it through the cell the unit stands on and does the work on it:
    /// the walk, the strike, the tint, the hit. This object only holds what those commands work on, and puts itself
    /// back the way it came when it returns to the pool.
    /// </summary>
    public class BoardUnit : PoolableItem, IOccupantView
    {
        [SerializeField] private SpriteRenderer _renderer;

        [Tooltip("The health bar, above the unit - hidden while the unit is unhurt.")]
        [SerializeField] private Transform _healthBar;

        [Tooltip("The bar's fill, pivoted on its left end: its x scale is the health left.")]
        [SerializeField] private Transform _healthFill;

        [Tooltip("The colour a hit flashes the sprite with.")]
        [SerializeField] private Color _hitFlash = new(1f, 0.35f, 0.35f, 1f);

        [Tooltip("Seconds a hit's flash takes, there and back.")]
        [Min(0.02f)] [SerializeField] private float _hitFlashDuration = 0.2f;

        public SpriteRenderer Renderer => _renderer;
        public Transform HealthBar => _healthBar;
        public Transform HealthFill => _healthFill;
        public Color HitFlash => _hitFlash;
        public float HitFlashDuration => _hitFlashDuration;

        /// <summary>The action under way - a walk, or a walk and a strike - so a new one can replace it.</summary>
        public Sequence Action { get; set; }

        /// <summary>The hit flash under way.</summary>
        public Tween Flash { get; set; }

        /// <summary>The colour the unit wears when not flashing: white, or the selection tint.</summary>
        public Color Tint { get; set; } = Color.white;

        public override void OnReturnToPool()
        {
            Action?.Kill();
            Action = null;
            Flash?.Kill();
            Flash = null;
            Tint = Color.white;
            _renderer.color = Color.white;
            _renderer.sprite = null;
            _healthBar.gameObject.SetActive(false);
            _healthFill.localScale = Vector3.one;
        }
    }
}
