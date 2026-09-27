using Modules.GridModule.Shared.Data.ValueObjects;
using Modules.UnitsModule.Entities;
using UnityEngine;

namespace Modules.UnitsModule.Data.ValueObjects
{
    /// <summary>
    /// A unit the units view puts on the board: the unit as the grid holds it, the pooled object that shows it, its
    /// sprite, the world rect of the cell it appears on, the world points it walks through and its speed in world
    /// units per second.
    /// </summary>
    internal readonly struct PlacedUnitVO
    {
        public readonly BoardUnitVO Unit;
        public readonly BoardUnit View;
        public readonly Sprite Sprite;
        public readonly Rect SpawnArea;
        public readonly Vector3[] Waypoints;
        public readonly float Speed;

        public PlacedUnitVO(BoardUnitVO unit, BoardUnit view, Sprite sprite, Rect spawnArea, Vector3[] waypoints, float speed)
        {
            Unit = unit;
            View = view;
            Sprite = sprite;
            SpawnArea = spawnArea;
            Waypoints = waypoints;
            Speed = speed;
        }
    }
}
