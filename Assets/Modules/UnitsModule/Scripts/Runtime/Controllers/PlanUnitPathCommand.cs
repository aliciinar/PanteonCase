using System.Collections.Generic;
using FlowIoC.BaseModule.Controller;
using FlowIoC.BaseModule.Injectable.Attributes;
using FlowIoC.ConsoleModule;
using Modules.GridModule.Services;
using Modules.GridModule.Shared.Data.ValueObjects;
using Modules.GridModule.Shared.Enums;
using Modules.UnitsModule.Data.ValueObjects;
using Modules.UnitsModule.Models;
using UnityEngine;

namespace Modules.UnitsModule.Controllers
{
    /// <summary>
    /// Plans how a unit gets from its building's door - the exit cell, a cell of the building - to its
    /// goal: the walk is the grid's A*, around buildings. The unit takes its goal right away, at full
    /// health - the grid holds its BoardUnitVO, the one copy of its data - so no other unit is sent there,
    /// and it takes nothing on the way. With no way out of the door to the goal, the unit appears on the
    /// goal itself rather than stay on the building.
    /// </summary>
    internal class PlanUnitPathCommand : Command<UnitSpawnVO>
    {
        [Inject] private IGridService _gridService { get; set; }
        [Inject] private IUnitsModel  _unitsModel  { get; set; }

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

            int hp = _unitsModel.Units[spawn.Request.Type].Hp;
            _gridService.Occupy(new RectInt(goal, Vector2Int.one), new BoardUnitVO(spawn.Request.Type, hp));

            Release(new UnitMoveVO(spawn.Request.Type, path));
        }
    }
}
