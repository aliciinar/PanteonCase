using System.Collections.Generic;
using FlowIoC.BaseModule.Controller;
using FlowIoC.BaseModule.Function.Provider;
using FlowIoC.BaseModule.Injectable.Attributes;
using FlowIoC.ConsoleModule;
using Modules.GridModule.Services;
using Modules.UnitsModule.Data.ValueObjects;
using Modules.UnitsModule.Shared.Data.ValueObjects;
using Modules.UnitsModule.Signals;
using UnityEngine;

namespace Modules.UnitsModule.Controllers
{
    /// <summary>
    /// Finds the cell a requested unit walks to: the building's spawn cell, kept on the grid, when it is
    /// free; otherwise the free cell nearest it. When no cell of the board is free the unit is not made:
    /// NoRoomForUnit is announced and the flow stops.
    /// </summary>
    internal class FindUnitGoalCommand : Command
    {
        [Inject]       private IGridService       _gridService      { get; set; }
        [Inject]       private IFunctionProvider  _functionProvider { get; set; }
        [InjectSignal] private UnitsSignals       _signals          { get; set; }
        [SignalParam]  private UnitSpawnRequestVO _request          { get; set; }

        public override void Execute()
        {
            Retain();

            Vector2Int spawn = _gridService.ClampArea(_request.SpawnCell, Vector2Int.one);
            Vector2Int? goal = _gridService.Cells[spawn.x, spawn.y].IsFree
                ? spawn
                : _functionProvider.Call<FindNearestFreeCellFunction>().AddParams(new List<Vector2Int> { spawn })
                                   .ExecuteAndGetResult<Vector2Int?>();

            if (goal == null)
            {
                FlowLogger.Log($"FindUnitGoalCommand - no free cell left on the board for a {_request.Type}.");
                _signals.Outgoing.NoRoomForUnit.Dispatch(_request.Type);
                Stop();
                return;
            }

            Release(new UnitSpawnVO(_request, goal.Value));
        }
    }
}
