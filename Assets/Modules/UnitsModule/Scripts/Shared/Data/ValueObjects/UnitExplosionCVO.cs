using System;
using UnityEngine;

namespace Modules.UnitsModule.Shared.Data.ValueObjects
{
    /// <summary>The puff a destroyed unit leaves where it stood: grows from a share of its width to all of it, fading.</summary>
    [Serializable]
    public class UnitExplosionCVO
    {
        [Tooltip("The puff shows one of these, picked at random.")]
        public Sprite[] Sprites;

        [Tooltip("How wide the puff grows, in cells.")]
        [Min(0.1f)] public float Size = 1.6f;

        [Tooltip("The share of its full width the puff starts at.")]
        [Range(0f, 1f)] public float StartScale = 0.4f;

        [Tooltip("Seconds the puff takes to grow and fade.")]
        [Min(0.05f)] public float Duration = 0.4f;
    }
}
