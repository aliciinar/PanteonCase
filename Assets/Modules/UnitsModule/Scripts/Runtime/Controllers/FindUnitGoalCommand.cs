using FlowIoC.BaseModule.Controller;
using FlowIoC.BaseModule.Injectable.Attributes;
using FlowIoC.ConsoleModule;
using Modules.GridModule.Services;
using Modules.UnitsModule.Data.ValueObjects;
using Modules.UnitsModule.Models;
using Modules.UnitsModule.Shared.Data.ValueObjects;
using UnityEngine;

namespace Modules.UnitsModule.Controllers
{
    /// <summary>
    /// Finds the cell a requested unit walks to: the building's spawn cell, kept on the grid, when it is
    /// free; otherwise the free cell nearest it. When no cell of the board is free the unit is not made and
    /// the flow stops. While an action runs (RD_GameStatus) no unit is made.
    /// </summary>
    internal class FindUnitGoalCommand : Command
    {
        [Inject]      private IGridService       _gridService { get; set; }
        [Inject]      private IUnitsModel        _unitsModel  { get; set; }
        [SignalParam] private UnitSpawnRequestVO _request     { get; set; }

        public override void Execute()
        {
            Retain();

            if (_unitsModel.IsGameLocked)
            {
                Stop();
                return;
            }

            // The search answers the spawn cell itself when it is free.
            Vector2Int spawn = _gridService.ClampArea(_request.SpawnCell, Vector2Int.one);
            Vector2Int? goal = _gridService.FindNearestFreeCellBfs(spawn);

            if (goal == null)
            {
                FlowLogger.Log($"FindUnitGoalCommand - no free cell left on the board for a {_request.Type}.");
                Stop();
                return;
            }

            Release(new UnitSpawnVO(_request, goal.Value));
        }
    }
}
