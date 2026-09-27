using Modules.GridModule.Shared.Data.ValueObjects;
using UnityEngine;

namespace Modules.UnitsModule.Data.ValueObjects
{
    /// <summary>A unit on the board the units view sets walking: which one, the world points it walks through and its speed in world units per second.</summary>
    internal readonly struct UnitWalkVO
    {
        public readonly BoardUnitVO Unit;
        public readonly Vector3[] Waypoints;
        public readonly float Speed;

        public UnitWalkVO(BoardUnitVO unit, Vector3[] waypoints, float speed)
        {
            Unit = unit;
            Waypoints = waypoints;
            Speed = speed;
        }
    }
}
