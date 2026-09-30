using System.Collections.Generic;
using FlowIoC.BaseModule.Controller;
using FlowIoC.BaseModule.Injectable.Attributes;
using FlowIoC.ConsoleModule;
using Modules.GridModule.Services;
using Modules.GridModule.Shared.Data.ValueObjects;
using Modules.GridModule.Shared.Enums;
using Modules.UnitsModule.Data.ValueObjects;
using Modules.UnitsModule.Models;
using Modules.UnitsModule.Signals;
using UnityEngine;

namespace Modules.UnitsModule.Controllers
{
    /// <summary>
    /// The selected unit is ordered to attack a building or a unit. A strike reaches one cell, so the unit strikes
    /// from a cell right next to its target: where it stands when it already is next to it, otherwise the free cell
    /// next to the target it can walk to soonest - one A* around buildings, whose goal is any side of the target, so
    /// a side walled off never hides one that is open. The unit takes that cell right away. With no unit selected,
    /// or the unit ordered at itself, nothing happens; with no cell next to the target it can walk to, the order is
    /// refused - OrderRefused carries CD_Units' no-way message - and the unit stays.
    /// </summary>
    internal class PlanAttackCommand : Command
    {
        [Inject]       private IGridService        _gridService    { get; set; }
        [Inject]       private IUnitSelectionModel _selectionModel { get; set; }
        [Inject]       private IUnitsModel         _unitsModel     { get; set; }
        [InjectSignal] private UnitsSignals        _signals        { get; set; }
        [SignalParam]  private CellOccupantVO      _target         { get; set; }

        public override void Execute()
        {
            Retain();

            BoardUnitVO attacker = _selectionModel.Selected;
            if (attacker == null || attacker == _target)
            {
                Stop();
                return;
            }

            List<Vector2Int> path = _gridService.FindPathNextToAStar(attacker.Cell, _target.Area, CellOccupantType.Building);
            if (path == null)
            {
                FlowLogger.Log($"PlanAttackCommand - the {attacker.Type} at {attacker.Cell} can walk to no cell next to {_target}; it stays.");
                _signals.Outgoing.OrderRefused.Dispatch(_unitsModel.NoWayMessage);
                Stop();
                return;
            }

            Vector2Int strikeCell = path[^1];
            if (strikeCell != attacker.Cell) _gridService.MoveOccupant(attacker, strikeCell);
            Release(new UnitAttackPlanVO(attacker, _target, path));
        }
    }
}
