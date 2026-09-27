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
    /// The selected unit is ordered to a free cell: the walk is the grid's A* from the cell it stands on, around
    /// buildings - units walk through units, as they do out of a door. The unit takes its new cell right away, so no
    /// other unit is sent there, and frees the one it held. With no unit selected, or no way to the cell - it is
    /// walled in by buildings - nothing happens.
    /// </summary>
    internal class PlanUnitMoveCommand : Command
    {
        [Inject]      private IGridService        _gridService    { get; set; }
        [Inject]      private IUnitSelectionModel _selectionModel { get; set; }
        [SignalParam] private Vector2Int          _target         { get; set; }

        public override void Execute()
        {
            Retain();

            BoardUnitVO unit = _selectionModel.Selected;
            if (unit == null)
            {
                Stop();
                return;
            }

            List<Vector2Int> path = _gridService.FindPathAStar(unit.Cell, _target, CellOccupantType.Building);
            if (path == null)
            {
                FlowLogger.Log($"PlanUnitMoveCommand - the {unit.Type} has no way from {unit.Cell} to {_target}; it stays.");
                Stop();
                return;
            }

            _gridService.MoveOccupant(unit, _target);
            Release(new UnitMoveVO(unit, path));
        }
    }
}
