using FlowIoC.BaseModule.Controller;
using FlowIoC.BaseModule.Function.Provider;
using FlowIoC.BaseModule.Injectable.Attributes;
using Modules.GameBoardModule.Data.ValueObjects;
using Modules.GameBoardModule.Models;
using UnityEngine;

namespace Modules.GameBoardModule.Controllers
{
    /// <summary>
    /// The board was pressed while a placement waits. Pressed on the building itself, it is grabbed
    /// where it was pressed and stays put; pressed on another cell, it comes to that cell, centred on
    /// it. Either way it follows the pointer until the press ends. A press off the grid is not the
    /// board's.
    /// </summary>
    internal class GrabPlacementCommand : Command
    {
        [Inject]      private IGameBoardModel   _gameBoardModel   { get; set; }
        [Inject]      private IFunctionProvider _functionProvider { get; set; }
        [SignalParam] private Vector2           _pointer          { get; set; }

        public override void Execute()
        {
            Retain();

            BuildingPlacementVO placement = _gameBoardModel.PendingPlacement;
            Vector2Int cell = _gameBoardModel.WorldToCell(_pointer);

            if (placement == null || !_gameBoardModel.IsInside(cell))
            {
                Stop();
                return;
            }

            _gameBoardModel.PlacementGrabOffset = placement.Area.Contains(cell)
                ? placement.Area.position - cell
                : -(placement.Area.size / 2);
            _gameBoardModel.IsDraggingPlacement = true;

            Vector2Int origin = _functionProvider.Call<ClampAreaToBoardFunction>()
                                                 .AddParams(cell + _gameBoardModel.PlacementGrabOffset, placement.Area.size)
                                                 .ExecuteAndGetResult<Vector2Int>();

            Release(new BuildingPlacementVO(placement.Type, new RectInt(origin, placement.Area.size)));
        }
    }
}
