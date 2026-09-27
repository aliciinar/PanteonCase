using FlowIoC.BaseModule.Controller;
using FlowIoC.BaseModule.Injectable.Attributes;
using FlowIoC.PoolModule.Services;
using Modules.BuildingsModule.Entities;
using Modules.GridModule.Shared.Data.ValueObjects;

namespace Modules.BuildingsModule.Controllers.BoardBuildings
{
    /// <summary>
    /// A building was destroyed - the grid has already taken it off the board: its object - reached through its data -
    /// goes back to the pool (group "board_buildings").
    /// </summary>
    internal class ReturnBoardBuildingCommand : Command
    {
        [Inject]      private IPoolService    _poolService { get; set; }
        [SignalParam] private BoardBuildingVO _building    { get; set; }

        public override void Execute() => _poolService.Return.Item((BoardBuilding)_building.View);
    }
}
