using Modules.UnitsModule.Entities;
using Modules.UnitsModule.Shared.Enums;
using UnityEngine;

namespace Modules.UnitsModule.Data.ValueObjects
{
    /// <summary>
    /// A unit the units view puts on the board: the pooled object, which unit it is, its sprite, the world
    /// rect of the cell it appears on, the world points it walks through and its speed in cells per second.
    /// </summary>
    internal readonly struct PlacedUnitVO
    {
        public readonly BoardUnit Unit;
        public readonly UnitType Type;
        public readonly Sprite Sprite;
        public readonly Rect SpawnArea;
        public readonly Vector3[] Waypoints;
        public readonly float Speed;

        public PlacedUnitVO(BoardUnit unit, UnitType type, Sprite sprite, Rect spawnArea, Vector3[] waypoints, float speed)
        {
            Unit = unit;
            Type = type;
            Sprite = sprite;
            SpawnArea = spawnArea;
            Waypoints = waypoints;
            Speed = speed;
        }
    }
}
