using System.Collections.Generic;
using FlowIoC.BaseModule.Function.ReturnableFunctions;
using FlowIoC.BaseModule.Injectable.Attributes;
using Modules.GridModule.Services;
using UnityEngine;

namespace Modules.UnitsModule.Controllers.BoardUnits
{
    /// <summary>The world centre of every cell of a path from the given index on - where a unit walks through.</summary>
    internal class CellsToWaypointsFunction : FunctionReturn<Vector3[], IReadOnlyList<Vector2Int>, int>
    {
        [Inject] private IGridService _gridService { get; set; }

        public override Vector3[] Execute(IReadOnlyList<Vector2Int> path, int from)
        {
            var waypoints = new Vector3[path.Count - from];
            for (int i = from; i < path.Count; i++)
                waypoints[i - from] = _gridService.AreaToWorldRect(new RectInt(path[i], Vector2Int.one)).center;

            return waypoints;
        }
    }
}
