using FlowIoC.BaseModule.Controller;
using FlowIoC.BaseModule.Injectable.Attributes;
using FlowIoC.PoolModule.Services;
using Modules.BuildingsModule.Data.ValueObjects;
using Modules.BuildingsModule.Entities;
using Modules.BuildingsModule.Models;
using Modules.BuildingsModule.Signals;
using Modules.GridModule.Services;
using UnityEngine;

namespace Modules.BuildingsModule.Controllers
{
    /// <summary>
    /// Shows a placed building over its area. The placement arrives from the step before it; its cells
    /// are turned into the world rect they cover, a building comes out of the pool (group
    /// "board_buildings", warmed while the game loaded) and the buildings view puts it there.
    /// </summary>
    internal class ShowBuildingCommand : Command<BuildingPlacementVO>
    {
        /// <summary>The key of the building in CD_PoolGroup_BoardBuildings.</summary>
        private const string BuildingPoolKey = "board_building";

        [Inject]       private IBuildingsModel          _buildingsModel  { get; set; }
        [Inject]       private IGridService             _gridService     { get; set; }
        [Inject]       private IPoolService             _poolService     { get; set; }
        [InjectSignal] private BuildingsInternalSignals _internalSignals { get; set; }

        public override void Execute(BuildingPlacementVO placement)
        {
            Rect area = _gridService.AreaToWorldRect(placement.Area);
            var building = _poolService.Get<BoardBuilding>(BuildingPoolKey, null);
            Sprite sprite = _buildingsModel.Buildings[placement.Type].BoardSprite;

            _internalSignals.ShowBuilding.Dispatch(new PlacedBuildingVO(building, placement.Type, sprite, area));
        }
    }
}
