using FlowIoC.PoolModule.Entities;
using UnityEngine;

namespace Modules.BuildingsModule.Entities
{
    /// <summary>
    /// A building standing on the board. Pooled (group "board_buildings"), so the same few objects are
    /// reused as buildings come and go; what it looks like is its BuildingSprite's.
    /// </summary>
    [RequireComponent(typeof(BuildingSprite))]
    public class BoardBuilding : PoolableItem
    {
        [SerializeField] private BuildingSprite _sprite;

        /// <summary>Draws the building's sprite over the world rect it covers.</summary>
        public void Show(Sprite sprite, Rect area) => _sprite.Show(sprite, area);

        public override void OnReturnToPool() => _sprite.Clear();
    }
}
