using DG.Tweening;
using FlowIoC.BaseModule.Controller;
using FlowIoC.BaseModule.Function.Provider;
using FlowIoC.BaseModule.Injectable.Attributes;
using Modules.GridModule.Services;
using Modules.UnitsModule.Data.ValueObjects;
using Modules.UnitsModule.Entities;
using Modules.UnitsModule.Models;
using Modules.UnitsModule.Shared.Data.ValueObjects;
using Modules.UnitsModule.Signals;
using UnityEngine;

namespace Modules.UnitsModule.Controllers.BoardUnits
{
    /// <summary>
    /// Plays an attack on the attacker object - reached through its data: it walks its planned cells to the cell it
    /// strikes from, lunges half-way at the nearest point of its target and back, taking the strike duration CD_Units
    /// gives. The strike lands as the lunge reaches the target; the lunge ending ends the action.
    /// </summary>
    internal class StrikeWithBoardUnitCommand : Command<UnitAttackPlanVO>
    {
        [Inject]       private IUnitsModel          _unitsModel       { get; set; }
        [Inject]       private IGridService         _gridService      { get; set; }
        [Inject]       private IFunctionProvider    _functionProvider { get; set; }
        [InjectSignal] private UnitsInternalSignals _internalSignals  { get; set; }

        public override void Execute(UnitAttackPlanVO plan)
        {
            var view = (BoardUnit)plan.Attacker.View;
            UnitCVO config = _unitsModel.Units[plan.Attacker.Type];

            var waypoints = _functionProvider.Call<CellsToWaypointsFunction>().AddParams(plan.Path, 1)
                                             .ExecuteAndGetResult<Vector3[]>();
            Vector3 standPoint = _gridService.AreaToWorldRect(new RectInt(plan.Path[plan.Path.Count - 1], Vector2Int.one)).center;
            Rect target = _gridService.AreaToWorldRect(plan.Target.Area);
            var strikePoint = new Vector3(Mathf.Clamp(standPoint.x, target.xMin, target.xMax),
                                          Mathf.Clamp(standPoint.y, target.yMin, target.yMax));
            Vector3 lungePoint = Vector3.Lerp(standPoint, strikePoint, 0.5f);
            float half = config.StrikeDuration * 0.5f;

            UnitsInternalSignals signals = _internalSignals;
            var strike = new UnitStrikeVO(plan.Attacker, plan.Target);

            view.Action?.Kill();
            view.Action = DOTween.Sequence().OnComplete(() => signals.UnitActionFinished.Dispatch());
            _functionProvider.Call<AppendWalkFunction>()
                             .AddParams(view.Action, view, waypoints, config.MoveSpeed * _gridService.CellSize)
                             .Execute();
            view.Action.Append(view.transform.DOMove(lungePoint, half).SetEase(Ease.OutQuad))
                       .AppendCallback(() => signals.UnitStruck.Dispatch(strike))
                       .Append(view.transform.DOMove(standPoint, half).SetEase(Ease.InQuad));
        }
    }
}
