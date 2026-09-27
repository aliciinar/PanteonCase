using System;
using UnityEngine;

namespace Modules.UnitsModule.Shared.Data.ValueObjects
{
    /// <summary>One unit as the designer authors it: its name, how it looks, how much health it has and how hard it hits.</summary>
    [Serializable]
    public class UnitCVO
    {
        [Tooltip("The name the player reads - on the production card and in the information panel.")]
        public string Name;

        [Tooltip("The unit on the board and in the information panel. A unit covers one cell.")]
        public Sprite Sprite;

        [Tooltip("Health points; the unit is destroyed when they reach 0.")]
        [Min(1)] public int Hp = 10;

        [Tooltip("Damage dealt per attack.")]
        [Min(0)] public int Damage = 1;

        [Tooltip("Walking speed in cells per second.")]
        [Min(0.1f)] public float MoveSpeed = 4f;

        [Tooltip("Seconds a strike takes - the lunge at the target and back. The game waits for it, like for a walk.")]
        [Min(0.05f)] public float StrikeDuration = 0.3f;
    }
}
