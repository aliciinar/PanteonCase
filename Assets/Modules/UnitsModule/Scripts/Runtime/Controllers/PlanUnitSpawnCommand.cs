using System.Collections.Generic;
using FlowIoC.BaseModule.Controller;
using FlowIoC.BaseModule.Injectable.Attributes;
using FlowIoC.ConsoleModule;
using Modules.GridModule.Services;
using Modules.GridModule.Shared.Enums;
using Modules.UnitsModule.Data.ValueObjects;
using Modules.UnitsModule.Models;
using Modules.UnitsModule.Shared.Data.ValueObjects;
using Modules.UnitsModule.Signals;
using UnityEngine;

namespace Modules.UnitsModule.Controllers
{
    /// <summary>
    /// Plans a requested unit's way out: from its building's door - the exit cell, a cell of the building - to the
    /// building's spawn cell, kept on the grid, or when that is taken or walled off, to the free cell nearest it that
    /// can be walked to. One breadth-first search out of the door, around buildings, finds both the cell and the walk.
    /// When no free cell can be walked to from the door, the unit is not made: OrderRefused carries CD_Units'
    /// no-room message. While an action runs (RD_GameStatus) no unit is made. The next step puts the unit on the board.
    /// </summary>
    internal class PlanUnitSpawnCommand : Command
    {
        [Inject]       private IGridService       _gridService { get; set; }
        [Inject]       private IUnitsModel        _unitsModel  { get; set; }
        [InjectSignal] private UnitsSignals       _signals     { get; set; }
        [SignalParam]  private UnitSpawnRequestVO _request     { get; set; }

        public override void Execute()
        {
            Retain();

            if (_unitsModel.IsGameLocked)
            {
                Stop();
                return;
            }

            Vector2Int spawn = _gridService.ClampArea(_request.SpawnCell, Vector2Int.one);

            // Buildings block a walk, units do not: a unit standing in front of the door would otherwise
            // shut it for every unit after it, and each unit still stops on a cell of its own.
            List<Vector2Int> path = _gridService.FindPathToNearestFreeCellBfs(_request.ExitCell, spawn, CellOccupantType.Building);

            if (path == null)
            {
                FlowLogger.Log($"PlanUnitSpawnCommand - a {_request.Type} can walk from its door {_request.ExitCell} to no free cell; it is not made.");
                _signals.Outgoing.OrderRefused.Dispatch(_unitsModel.NoRoomToSpawnMessage);
                Stop();
                return;
            }

            Release(new UnitSpawnPlanVO(_request.Type, path));
        }
    }
}
