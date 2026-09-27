using Modules.BuildingsModule.Data.ValueObjects;
using UnityEngine;

namespace Modules.BuildingsModule.Models
{
    internal class PlacementModel : IPlacementModel
    {
        public BuildingPlacementVO Pending { get; private set; }
        public bool IsWaiting => Pending != null;
        public bool IsDragging { get; private set; }
        public Vector2Int GrabOffset { get; private set; }

        public void Wait(BuildingPlacementVO placement) => Pending = placement;

        public void Grab(Vector2Int grabOffset)
        {
            GrabOffset = grabOffset;
            IsDragging = true;
        }

        public void Release() => IsDragging = false;

        public BuildingPlacementVO Take()
        {
            BuildingPlacementVO placement = Pending;
            Discard();
            return placement;
        }

        public void Discard()
        {
            Pending = null;
            IsDragging = false;
        }
    }
}
