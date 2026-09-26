using System.Collections.Generic;
using FlowIoC.BaseModule.Controller;
using FlowIoC.BaseModule.Function.Provider;
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
    /// goal: the walk is A*'s, around buildings and standing units. The unit claims its goal right away,
    /// so no other unit is sent there, and claims nothing on the way. With no way out of the door to the
    /// goal, the unit appears on the goal itself rather than stay on the building.
    /// </summary>
    internal class PlanUnitPathCommand : Command<UnitSpawnVO>
    {
        [Inject] private IGridService      _gridService      { get; set; }
        [Inject] private IFunctionProvider _functionProvider { get; set; }

        public override void Execute(UnitSpawnVO spawn)
        {
            Retain();

            Vector2Int door = spawn.Request.ExitCell;
            Vector2Int goal = spawn.GoalCell;

            List<Vector2Int> path = _functionProvider.Call<FindPathFunction>().AddParams(door, goal)
                                                     .ExecuteAndGetResult<List<Vector2Int>>();

            if (path == null)
            {
                FlowLogger.Log($"PlanUnitPathCommand - the {spawn.Request.Type} has no way from its door to {goal}; it appears there.");
                path = new List<Vector2Int> { goal };
            }

            _gridService.Occupy(new RectInt(goal, Vector2Int.one), CellOccupantType.Soldier);

            Release(new UnitMoveVO(spawn.Request.Type, path));
        }
    }
}
