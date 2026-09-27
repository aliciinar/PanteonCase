using System.Collections.Generic;
using FlowIoC.BaseModule.Controller;
using FlowIoC.BaseModule.Injectable.Attributes;
using FlowIoC.ConsoleModule;
using Modules.GridModule.Services;
using Modules.GridModule.Shared.Enums;
using Modules.UnitsModule.Data.ValueObjects;
using UnityEngine;

namespace Modules.UnitsModule.Controllers
{
    /// <summary>
    /// Plans how a unit gets from its building's door - the exit cell, a cell of the building - to its
    /// goal: the walk is the grid's A*, around buildings. With no way out of the door to the goal, the unit
    /// appears on the goal itself rather than stay on the building. The next step puts the unit on the board.
    /// </summary>
    internal class PlanUnitPathCommand : Command<UnitSpawnVO>
    {
        [Inject] private IGridService _gridService { get; set; }

        public override void Execute(UnitSpawnVO spawn)
        {
            Retain();

            Vector2Int door = spawn.Request.ExitCell;
            Vector2Int goal = spawn.GoalCell;

            // Buildings block a walk, units do not: a unit standing in front of the door would otherwise
            // shut it for every unit after it, and each unit still stops on a cell of its own.
            List<Vector2Int> path = _gridService.FindPathAStar(door, goal, CellOccupantType.Building);

            if (path == null)
            {
                FlowLogger.Log($"PlanUnitPathCommand - the {spawn.Request.Type} has no way from its door to {goal}; it appears there.");
                path = new List<Vector2Int> { goal };
            }

            Release(new UnitSpawnPlanVO(spawn.Request.Type, path));
        }
    }
}
