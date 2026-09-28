using UnityEngine;

namespace Modules.UnitsModule.Data.ValueObjects
{
    /// <summary>How a destroyed unit's puff looks, as CD_Units authors it: its sprites, its width in cells and how long it lasts.</summary>
    internal readonly struct UnitExplosionVO
    {
        public readonly Sprite[] Sprites;
        public readonly float Size;
        public readonly float Duration;

        public UnitExplosionVO(Sprite[] sprites, float size, float duration)
        {
            Sprites = sprites;
            Size = size;
            Duration = duration;
        }
    }
}
