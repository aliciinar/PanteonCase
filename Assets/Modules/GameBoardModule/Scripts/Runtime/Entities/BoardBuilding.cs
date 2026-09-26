using FlowIoC.PoolModule.Entities;
using UnityEngine;

namespace Modules.GameBoardModule.Entities
{
    /// <summary>
    /// A building standing on the board: one sprite renderer. Pooled (group "gameboard"), so the same
    /// few objects are reused as buildings come and go.
    /// </summary>
    public class BoardBuilding : PoolableItem
    {
        [SerializeField] private SpriteRenderer _renderer;

        /// <summary>
        /// Draws the sprite over a world rect. The sprite is scaled to the rect, so the footprint in the
        /// data decides the size on the board, never the sprite's own pixel size.
        /// </summary>
        public void Show(Sprite sprite, Rect area)
        {
            _renderer.sprite = sprite;

            Vector2 spriteSize = sprite.bounds.size;
            transform.position = area.center;
            transform.localScale = new Vector3(area.width / spriteSize.x, area.height / spriteSize.y, 1f);
        }

        public override void OnReturnToPool() => _renderer.sprite = null;
    }
}
