using DG.Tweening;
using FlowIoC.PoolModule.Entities;
using UnityEngine;

namespace Modules.UnitsModule.Entities
{
    /// <summary>
    /// A unit on the board: one sprite renderer that walks. Pooled (group "units"), so the same objects
    /// are reused as units come and go. Walking is a tween along the planned cells at a steady speed -
    /// how the unit moves is presentation; where it goes was decided before it got here.
    /// </summary>
    public class BoardUnit : PoolableItem
    {
        [SerializeField] private SpriteRenderer _renderer;

        private Tween _walk;

        /// <summary>
        /// Draws the sprite over a world rect - the cell the unit stands on. The sprite is scaled to the
        /// rect, so a unit covers one cell whatever the sprite's own pixel size.
        /// </summary>
        public void Show(Sprite sprite, Rect area)
        {
            _renderer.sprite = sprite;

            Vector2 spriteSize = sprite.bounds.size;
            transform.position = area.center;
            transform.localScale = new Vector3(area.width / spriteSize.x, area.height / spriteSize.y, 1f);
        }

        /// <param name="waypoints">The centre of every cell to walk through, one cell apart, the last where it stops.</param>
        /// <param name="cellsPerSecond">Walking speed.</param>
        public void MoveAlong(Vector3[] waypoints, float cellsPerSecond)
        {
            _walk?.Kill();
            _walk = transform.DOPath(waypoints, waypoints.Length / cellsPerSecond, PathType.Linear)
                             .SetEase(Ease.Linear);
        }

        public override void OnReturnToPool()
        {
            _walk?.Kill();
            _walk = null;
            _renderer.sprite = null;
        }
    }
}
