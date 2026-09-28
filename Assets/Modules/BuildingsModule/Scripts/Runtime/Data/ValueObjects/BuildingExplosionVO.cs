using UnityEngine;

namespace Modules.BuildingsModule.Data.ValueObjects
{
    /// <summary>
    /// How a destroyed building explodes, as CD_Buildings authors it: its sprites, how many puffs, each puff's width as a
    /// share of the footprint's longer side, the seconds between puffs and how long each lasts.
    /// </summary>
    internal readonly struct BuildingExplosionVO
    {
        public readonly Sprite[] Sprites;
        public readonly int Count;
        public readonly float Size;
        public readonly float Stagger;
        public readonly float Duration;

        public BuildingExplosionVO(Sprite[] sprites, int count, float size, float stagger, float duration)
        {
            Sprites = sprites;
            Count = count;
            Size = size;
            Stagger = stagger;
            Duration = duration;
        }
    }
}
