using FlowIoC.BaseModule.Controller;
using FlowIoC.BaseModule.Injectable.Attributes;
using Modules.BuildingsModule.Data.ValueObjects;
using Modules.BuildingsModule.Models;
using Modules.GridModule.Services;
using UnityEngine;

namespace Modules.BuildingsModule.Controllers
{
    /// <summary>
    /// The board was pressed. With no placement waiting the press is not the placement's and the flow
    /// stops. Pressed on the waiting building itself, it is grabbed where it was pressed and stays put;
    /// pressed on another cell, it comes to that cell, centred on it. Either way it follows the pointer
    /// until the press ends. A press off the grid is not the board's.
    /// </summary>
    internal class GrabPlacementCommand : Command
    {
        [Inject]      private IPlacementModel _placementModel { get; set; }
        [Inject]      private IGridService    _gridService    { get; set; }
        [SignalParam] private Vector2         _pointer        { get; set; }

        public override void Execute()
        {
            Retain();

            if (!_placementModel.IsWaiting)
            {
                Stop();
                return;
            }

            BuildingPlacementVO placement = _placementModel.Pending;
            Vector2Int cell = _gridService.WorldToCell(_pointer);

            if (!_gridService.IsInside(cell))
            {
                Stop();
                return;
            }

            // Kept by the model: every drag of this press places the building by it.
            Vector2Int grabOffset = placement.Area.Contains(cell)
                ? placement.Area.position - cell
                : -(placement.Area.size / 2);
            _placementModel.Grab(grabOffset);

            Vector2Int origin = _gridService.ClampArea(cell + grabOffset, placement.Area.size);
            Release(new BuildingPlacementVO(placement.Type, new RectInt(origin, placement.Area.size)));
        }
    }
}
