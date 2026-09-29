using UnityEngine;

namespace Modules.BuildingsModule.Entities
{
    /// <summary>
    /// A building's sprite on the board, fitted to the world rect it covers. Shared by the placed
    /// buildings and by the placement preview's ghost, which also colours it. A struck building's flash is played on its
    /// renderer by the command that shows the hit.
    /// </summary>
    public class BuildingSprite : MonoBehaviour
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

        /// <summary>Colours the sprite - how the placement ghost shows whether it fits.</summary>
        public void SetColor(Color color) => _renderer.color = color;

        public SpriteRenderer Renderer => _renderer;

        public void Clear() => _renderer.sprite = null;
    }
}
