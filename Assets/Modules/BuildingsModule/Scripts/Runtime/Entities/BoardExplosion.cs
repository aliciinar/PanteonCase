using DG.Tweening;
using FlowIoC.PoolModule.Entities;
using UnityEngine;

namespace Modules.BuildingsModule.Entities
{
    /// <summary>
    /// One puff of a destroyed building's explosion: a sprite that grows and fades over the footprint. Pooled (group
    /// "board_buildings", key "building_explosion" - pool keys are global, so each module names its own); the command that plays it hands it back to the pool when the puff is
    /// over.
    /// </summary>
    public class BoardExplosion : PoolableItem
    {
        [SerializeField] private SpriteRenderer _renderer;

        public SpriteRenderer Renderer => _renderer;

        /// <summary>The puff under way.</summary>
        public Tween Puff { get; set; }

        public override void OnReturnToPool()
        {
            Puff?.Kill();
            Puff = null;
            _renderer.color = Color.white;
            _renderer.sprite = null;
            transform.localScale = Vector3.one;
            transform.rotation = Quaternion.identity;
        }
    }
}
