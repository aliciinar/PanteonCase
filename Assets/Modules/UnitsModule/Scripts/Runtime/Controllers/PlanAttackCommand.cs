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
    /// The selected unit is ordered to attack a building or a unit. A strike reaches one cell, so the unit strikes
    /// from a cell right next to its target: where it stands when it already is next to it, otherwise the free cell
    /// next to the target nearest it (the grid's breadth-first search out of the target's cells), walked to by A*
    /// around buildings. The unit takes that cell right away. With no unit selected, the unit ordered at itself, no
    /// free cell next to the target, or no way there, nothing happens.
    /// </summary>
    internal class PlanAttackCommand : Command
    {
        [Inject]      private IGridService        _gridService    { get; set; }
        [Inject]      private IUnitSelectionModel _selectionModel { get; set; }
        [SignalParam] private CellOccupantVO      _target         { get; set; }

        public override void Execute()
        {
            Retain();

            BoardUnitVO attacker = _selectionModel.Selected;
            if (attacker == null || attacker == _target)
            {
                Stop();
                return;
            }

            RectInt area = _target.Area;
            Vector2Int? strikeCell = IsNextTo(attacker.Cell, area)
                ? attacker.Cell
                : _gridService.FindFreeCellAroundBfs(area, attacker.Cell);

            if (strikeCell == null)
            {
                FlowLogger.Log($"PlanAttackCommand - no free cell next to {_target}; the {attacker.Type} cannot reach it.");
                Stop();
                return;
            }

            List<Vector2Int> path = _gridService.FindPathAStar(attacker.Cell, strikeCell.Value, CellOccupantType.Building);
            if (path == null)
            {
                FlowLogger.Log($"PlanAttackCommand - the {attacker.Type} has no way from {attacker.Cell} to {strikeCell.Value}; it stays.");
                Stop();
                return;
            }

            if (strikeCell.Value != attacker.Cell) _gridService.MoveOccupant(attacker, strikeCell.Value);
            Release(new UnitAttackPlanVO(attacker, _target, path));
        }

        /// <summary>Whether the cell is outside the area and one neighbour step from one of its cells.</summary>
        private static bool IsNextTo(Vector2Int cell, RectInt area)
        {
            if (area.Contains(cell)) return false;

            int nearestX = Mathf.Clamp(cell.x, area.xMin, area.xMax - 1);
            int nearestY = Mathf.Clamp(cell.y, area.yMin, area.yMax - 1);
            return Mathf.Abs(cell.x - nearestX) + Mathf.Abs(cell.y - nearestY) == 1;
        }
    }
}
