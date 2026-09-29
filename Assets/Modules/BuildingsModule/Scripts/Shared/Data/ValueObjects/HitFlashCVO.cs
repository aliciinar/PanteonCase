using System;
using UnityEngine;

namespace Modules.BuildingsModule.Shared.Data.ValueObjects
{
    /// <summary>How a struck building flashes: the colour it flashes with and how long the flash takes, there and back.</summary>
    [Serializable]
    public class HitFlashCVO
    {
        [Tooltip("The colour a struck building's sprite flashes with. A vertex colour, so it still batches.")]
        public Color Color = new(1f, 0.35f, 0.35f, 1f);

        [Tooltip("Seconds a hit's flash takes, there and back.")]
        [Min(0.02f)] public float Duration = 0.2f;
    }
}
