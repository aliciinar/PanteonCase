using FlowIoC.BaseModule.Controller;
using FlowIoC.BaseModule.Injectable.Attributes;
using FlowIoC.PoolModule.Services;
using Modules.GridModule.Services;
using Modules.UnitsModule.Data.ValueObjects;
using Modules.UnitsModule.Entities;
using Modules.UnitsModule.Models;
using Modules.UnitsModule.Shared.Data.ValueObjects;
using Modules.UnitsModule.Signals;
using UnityEngine;

namespace Modules.UnitsModule.Controllers
{
    /// <summary>
    /// Puts a unit on the board and sets it walking. The unit and its planned cells arrive from the step
    /// before it and the cells are turned into world points - the first one where it appears, the rest
    /// where it walks - a unit comes out of the pool (group "units", warmed while the game loaded) and the
    /// units view does the rest. Speed is CD_Units' cells per second, in world units.
    /// </summary>
    internal class ShowUnitCommand : Command<UnitMoveVO>
    {
        /// <summary>The key of the unit in CD_PoolGroup_Units.</summary>
        private const string UnitPoolKey = "board_unit";

        [Inject]       private IUnitsModel          _unitsModel      { get; set; }
        [Inject]       private IGridService         _gridService     { get; set; }
        [Inject]       private IPoolService         _poolService     { get; set; }
        [InjectSignal] private UnitsInternalSignals _internalSignals { get; set; }

        public override void Execute(UnitMoveVO move)
        {
            UnitCVO unit = _unitsModel.Units[move.Unit.Type];
            Rect spawnArea = CellRect(move.Path[0]);

            var waypoints = new Vector3[move.Path.Count - 1];
            for (int i = 1; i < move.Path.Count; i++)
                waypoints[i - 1] = CellRect(move.Path[i]).center;

            var boardUnit = _poolService.Get<BoardUnit>(UnitPoolKey, null);
            _internalSignals.ShowUnit.Dispatch(new PlacedUnitVO(move.Unit, boardUnit, unit.Sprite, spawnArea, waypoints,
                                                                unit.MoveSpeed * _gridService.CellSize));
        }

        private Rect CellRect(Vector2Int cell) => _gridService.AreaToWorldRect(new RectInt(cell, Vector2Int.one));
    }
}
