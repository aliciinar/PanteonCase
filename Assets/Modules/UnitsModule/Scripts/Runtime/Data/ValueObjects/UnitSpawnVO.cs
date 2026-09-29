using Modules.UnitsModule.Shared.Data.ValueObjects;
using UnityEngine;

namespace Modules.UnitsModule.Data.ValueObjects
{
    /// <summary>A unit request and the free cell the unit walks to. Handed from step to step of a spawn.</summary>
    internal class UnitSpawnVO
    {
        public UnitSpawnRequestVO Request { get; }
        public Vector2Int GoalCell { get; }

        public UnitSpawnVO(UnitSpawnRequestVO request, Vector2Int goalCell)
        {
            Request = request;
            GoalCell = goalCell;
        }
    }
}
