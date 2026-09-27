using DG.Tweening;
using FlowIoC.BaseModule.Function.VoidFunctions;
using Modules.UnitsModule.Entities;
using UnityEngine;

namespace Modules.UnitsModule.Controllers.BoardUnits
{
    /// <summary>
    /// Adds a unit's walk through the waypoints to an action's sequence, at a steady speed. A speed-based tween cannot
    /// sit in a sequence, so the walk takes the time its length needs at that speed. No waypoints, no walk.
    /// </summary>
    internal class AppendWalkFunction : FunctionVoid<Sequence, BoardUnit, Vector3[], float>
    {
        public override void Execute(Sequence action, BoardUnit unit, Vector3[] waypoints, float unitsPerSecond)
        {
            if (waypoints.Length == 0) return;

            float length = Vector3.Distance(unit.transform.position, waypoints[0]);
            for (int i = 1; i < waypoints.Length; i++)
                length += Vector3.Distance(waypoints[i - 1], waypoints[i]);

            action.Append(unit.transform.DOPath(waypoints, length / unitsPerSecond, PathType.Linear).SetEase(Ease.Linear));
        }
    }
}
