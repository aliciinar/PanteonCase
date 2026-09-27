using DG.Tweening;
using FlowIoC.BaseModule.Controller;
using FlowIoC.BaseModule.Function.Provider;
using FlowIoC.BaseModule.Injectable.Attributes;
using Modules.GridModule.Services;
using Modules.UnitsModule.Data.ValueObjects;
using Modules.UnitsModule.Entities;
using Modules.UnitsModule.Models;
using Modules.UnitsModule.Signals;
using UnityEngine;

namespace Modules.UnitsModule.Controllers.BoardUnits
{
    /// <summary>
    /// Walks a unit object - reached through the unit data - along its planned cells, every cell after the one it
    /// stands on, at the speed CD_Units gives. The walk ending ends the action.
    /// </summary>
    internal class WalkBoardUnitCommand : Command<UnitMoveVO>
    {
        [Inject]       private IUnitsModel          _unitsModel       { get; set; }
        [Inject]       private IGridService         _gridService      { get; set; }
        [Inject]       private IFunctionProvider    _functionProvider { get; set; }
        [InjectSignal] private UnitsInternalSignals _internalSignals  { get; set; }

        public override void Execute(UnitMoveVO move)
        {
            var view = (BoardUnit)move.Unit.View;
            var waypoints = _functionProvider.Call<CellsToWaypointsFunction>().AddParams(move.Path, 1)
                                             .ExecuteAndGetResult<Vector3[]>();
            float speed = _unitsModel.Units[move.Unit.Type].MoveSpeed * _gridService.CellSize;

            UnitsInternalSignals signals = _internalSignals;
            view.Action?.Kill();
            view.Action = DOTween.Sequence().OnComplete(() => signals.UnitActionFinished.Dispatch());
            _functionProvider.Call<AppendWalkFunction>().AddParams(view.Action, view, waypoints, speed).Execute();
        }
    }
}
