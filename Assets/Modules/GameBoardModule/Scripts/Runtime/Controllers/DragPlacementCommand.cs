using FlowIoC.BaseModule.Controller;
using FlowIoC.BaseModule.Function.Provider;
using FlowIoC.BaseModule.Injectable.Attributes;
using Modules.GameBoardModule.Data.ValueObjects;
using Modules.GameBoardModule.Models;
using UnityEngine;

namespace Modules.GameBoardModule.Controllers
{
    /// <summary>
    /// The pointer moved while a grabbed placement follows it: the placement moves with it, held by the
    /// cell it was grabbed at and kept inside the grid. Nothing is redrawn until it reaches a new cell.
    /// </summary>
    internal class DragPlacementCommand : Command
    {
        [Inject]      private IGameBoardModel   _gameBoardModel   { get; set; }
        [Inject]      private IFunctionProvider _functionProvider { get; set; }
        [SignalParam] private Vector2           _pointer          { get; set; }

        public override void Execute()
        {
            Retain();

            if (!_gameBoardModel.IsDraggingPlacement)
            {
                Stop();
                return;
            }

            BuildingPlacementVO placement = _gameBoardModel.PendingPlacement;
            Vector2Int cell = _gameBoardModel.WorldToCell(_pointer);
            Vector2Int origin = _functionProvider.Call<ClampAreaToBoardFunction>()
                                                 .AddParams(cell + _gameBoardModel.PlacementGrabOffset, placement.Area.size)
                                                 .ExecuteAndGetResult<Vector2Int>();

            if (origin == placement.Area.position)
            {
                Stop();
                return;
            }

            Release(new BuildingPlacementVO(placement.Type, new RectInt(origin, placement.Area.size)));
        }
    }
}
