using FlowIoC.PoolModule.Entities;
using UnityEngine;

namespace Modules.GameBoardModule.Entities
{
    /// <summary>
    /// One cell of the board on screen: a plain grey square whose darker edge, next to its neighbours',
    /// draws the grid. Pooled (group "board") - one per cell, all sharing one sprite and one material, so
    /// the whole grid batches together - and a cell of its own is what later lets a single cell be
    /// tinted.
    /// </summary>
    public class BoardCell : PoolableItem
    {
        [SerializeField] private SpriteRenderer _renderer;

        /// <param name="centre">World position of the cell's centre.</param>
        /// <param name="cellSize">Edge of one cell in world units; the sprite is scaled to it.</param>
        public void Show(Vector2 centre, float cellSize)
        {
            transform.position = centre;
            transform.localScale = Vector3.one * (cellSize / _renderer.sprite.bounds.size.x);
        }
    }
}
