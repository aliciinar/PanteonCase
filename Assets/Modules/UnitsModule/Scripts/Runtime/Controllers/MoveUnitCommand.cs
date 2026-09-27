using FlowIoC.BaseModule.Controller;
using FlowIoC.BaseModule.Injectable.Attributes;
using Modules.GridModule.Services;
using Modules.UnitsModule.Data.ValueObjects;
using Modules.UnitsModule.Models;
using Modules.UnitsModule.Signals;
using UnityEngine;

namespace Modules.UnitsModule.Controllers
{
    /// <summary>
    /// Sets a unit walking along its planned cells, turned into world points - every cell of the path, the first
    /// included, so a unit caught mid-step finishes into the cell it was stepping into before it turns. Speed is
    /// CD_Units' cells per second, in world units.
    /// </summary>
    internal class MoveUnitCommand : Command<UnitMoveVO>
    {
        [Inject]       private IUnitsModel          _unitsModel      { get; set; }
        [Inject]       private IGridService         _gridService     { get; set; }
        [InjectSignal] private UnitsInternalSignals _internalSignals { get; set; }

        public override void Execute(UnitMoveVO move)
        {
            var waypoints = new Vector3[move.Path.Count];
            for (int i = 0; i < move.Path.Count; i++)
                waypoints[i] = _gridService.AreaToWorldRect(new RectInt(move.Path[i], Vector2Int.one)).center;

            float speed = _unitsModel.Units[move.Unit.Type].MoveSpeed * _gridService.CellSize;
            _internalSignals.MoveUnit.Dispatch(new UnitWalkVO(move.Unit, waypoints, speed));
        }
    }
}
