using FlowIoC.BaseModule.Controller;
using FlowIoC.BaseModule.Injectable.Attributes;
using FlowIoC.PoolModule.Services;
using Modules.GridModule.Shared.Data.ValueObjects;
using Modules.UnitsModule.Entities;
using Modules.UnitsModule.Models;

namespace Modules.UnitsModule.Controllers.BoardUnits
{
    /// <summary>
    /// A unit was destroyed - the grid has already taken it off the board: it is no longer selected, if it was, and its
    /// object - reached through its data - goes back to the pool (group "units").
    /// </summary>
    internal class RemoveBoardUnitCommand : Command
    {
        [Inject]      private IUnitSelectionModel _selectionModel { get; set; }
        [Inject]      private IPoolService        _poolService    { get; set; }
        [SignalParam] private BoardUnitVO         _unit           { get; set; }

        public override void Execute()
        {
            if (_selectionModel.Selected == _unit) _selectionModel.ClearSelection();

            _poolService.Return.Item((BoardUnit)_unit.View);
        }
    }
}
