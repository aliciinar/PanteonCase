using System;
using UnityEngine;

namespace Modules.BuildingsModule.Shared.Data.ValueObjects
{
    /// <summary>
    /// How a destroyed building explodes: a number of puffs - the first at its centre, the rest at random points over its
    /// footprint, each a little after the one before - each growing from a share of its width to all of it, fading.
    /// </summary>
    [Serializable]
    public class BuildingExplosionCVO
    {
        [Tooltip("Each puff shows one of these, picked at random.")]
        public Sprite[] Sprites;

        [Tooltip("How many puffs a destroyed building explodes in: the first at its centre, the rest over its footprint.")]
        [Min(1)] public int Count = 3;

        [Tooltip("Each puff's width, as a share of the footprint's longer side.")]
        [Min(0.1f)] public float Size = 0.9f;

        [Tooltip("The share of its full width a puff starts at.")]
        [Range(0f, 1f)] public float StartScale = 0.4f;

        [Tooltip("How far from the footprint's centre the later puffs may land, as a share of its half-size.")]
        [Range(0f, 1f)] public float Spread = 0.6f;

        [Tooltip("Seconds between one puff and the next.")]
        [Min(0f)] public float Stagger = 0.08f;

        [Tooltip("Seconds a puff takes to grow and fade.")]
        [Min(0.05f)] public float Duration = 0.5f;
    }
}
