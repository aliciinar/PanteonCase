using FlowIoC.BaseModule.Attributes;
using FlowIoC.BaseModule.Controller;
using FlowIoC.BaseModule.Injectable.Attributes;
using Modules.BuildingsModule.Data.ValueObjects;
using Modules.BuildingsModule.Models;
using Modules.GridModule.Services;
using UnityEngine;

namespace Modules.BuildingsModule.Controllers
{
    /// <summary>
    /// The pointer moved while a grabbed placement follows it: the placement moves with it, held by the
    /// cell it was grabbed at and kept inside the grid. Nothing is redrawn until it reaches a new cell.
    /// Kept out of the Flow Console - it runs every frame of a drag.
    /// </summary>
    [HideCommandLog]
    internal class DragPlacementCommand : Command
    {
        [Inject]      private IPlacementModel _placementModel { get; set; }
        [Inject]      private IGridService    _gridService    { get; set; }
        [SignalParam] private Vector2         _pointer        { get; set; }

        public override void Execute()
        {
            Retain();

            if (!_placementModel.IsDragging)
            {
                Stop();
                return;
            }

            BuildingPlacementVO placement = _placementModel.PendingPlacement;
            Vector2Int cell = _gridService.WorldToCell(_pointer);
            Vector2Int origin = _gridService.ClampArea(cell + _placementModel.GrabOffset, placement.Area.size);

            if (origin == placement.Area.position)
            {
                Stop();
                return;
            }

            Release(new BuildingPlacementVO(placement.Type, new RectInt(origin, placement.Area.size)));
        }
    }
}
