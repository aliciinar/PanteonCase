using DG.Tweening;
using FlowIoC.BaseModule.Controller;
using FlowIoC.BaseModule.Function.Provider;
using FlowIoC.BaseModule.Injectable.Attributes;
using FlowIoC.PoolModule.Services;
using Modules.GridModule.Services;
using Modules.GridModule.Shared.Data.ValueObjects;
using Modules.UnitsModule.Data.ValueObjects;
using Modules.UnitsModule.Entities;
using Modules.UnitsModule.Models;
using Modules.UnitsModule.Shared.Data.ValueObjects;
using Modules.UnitsModule.Signals;
using UnityEngine;

namespace Modules.UnitsModule.Controllers.BoardUnits
{
    /// <summary>
    /// Puts a planned unit on the board. A unit object comes out of the pool (group "units", warmed while the game
    /// loaded) under the module's Root; the unit takes its goal at full health - the grid holds its BoardUnitVO, the one
    /// copy of its data, with this object as its View - so no other unit is sent there. The object appears on the door,
    /// its sprite fitted to one cell, and walks the planned cells at the speed CD_Units gives; the walk ending ends the
    /// action.
    /// </summary>
    internal class PlaceBoardUnitCommand : Command<UnitSpawnPlanVO>
    {
        /// <summary>The key of the unit in CD_PoolGroup_Units.</summary>
        private const string UnitPoolKey = "board_unit";

        [Inject]       private IUnitsModel          _unitsModel       { get; set; }
        [Inject]       private IGridService         _gridService      { get; set; }
        [Inject]       private IPoolService         _poolService      { get; set; }
        [Inject]       private IFunctionProvider    _functionProvider { get; set; }
        [InjectSignal] private UnitsInternalSignals _internalSignals  { get; set; }

        public override void Execute(UnitSpawnPlanVO plan)
        {
            UnitCVO config = _unitsModel.Units[plan.Type];
            Vector2Int goal = plan.Path[plan.Path.Count - 1];

            var view = _poolService.Get<BoardUnit>(UnitPoolKey, _unitsModel.BoardParent);
            _gridService.Occupy(new RectInt(goal, Vector2Int.one), new BoardUnitVO(plan.Type, config.Hp, goal, view));

            // The sprite is scaled to the cell, so a unit covers one cell whatever the sprite's own pixel size.
            Rect door = _gridService.AreaToWorldRect(new RectInt(plan.Path[0], Vector2Int.one));
            Vector2 spriteSize = config.Sprite.bounds.size;
            view.name = plan.Type.ToString();
            view.Renderer.sprite = config.Sprite;
            view.transform.position = door.center;
            view.transform.localScale = new Vector3(door.width / spriteSize.x, door.height / spriteSize.y, 1f);

            var waypoints = _functionProvider.Call<CellsToWaypointsFunction>().AddParams(plan.Path, 1)
                                             .ExecuteAndGetResult<Vector3[]>();

            // The action ends a frame later at the earliest, even with nothing to walk: the game waits for it.
            UnitsInternalSignals signals = _internalSignals;
            view.Action = DOTween.Sequence().OnComplete(() => signals.UnitActionFinished.Dispatch());
            _functionProvider.Call<AppendWalkFunction>()
                             .AddParams(view.Action, view, waypoints, config.MoveSpeed * _gridService.CellSize)
                             .Execute();
        }
    }
}
