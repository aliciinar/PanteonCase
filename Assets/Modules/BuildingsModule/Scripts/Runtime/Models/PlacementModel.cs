using Modules.BuildingsModule.Data.ValueObjects;
using UnityEngine;

namespace Modules.BuildingsModule.Models
{
    internal class PlacementModel : IPlacementModel
    {
        public BuildingPlacementVO PendingPlacement { get; set; }
        public bool IsDragging { get; set; }
        public Vector2Int GrabOffset { get; set; }
    }
}
