using FlowIoC.BaseModule.Controller;
using FlowIoC.BaseModule.Injectable.Attributes;
using FlowIoC.PoolModule.Services;
using Modules.BuildingsModule.Data.ValueObjects;
using Modules.BuildingsModule.Entities;
using Modules.BuildingsModule.Models;
using Modules.BuildingsModule.Shared.Data.ValueObjects;
using Modules.GridModule.Services;
using Modules.GridModule.Shared.Data.ValueObjects;
using UnityEngine;

namespace Modules.BuildingsModule.Controllers.BoardBuildings
{
    /// <summary>
    /// Puts the confirmed placement on the board. A building object comes out of the pool (group "board_buildings",
    /// warmed while the game loaded) under the module's Root; the building takes its cells at full health - every cell
    /// of its area holds the same BoardBuildingVO, the one copy of its data, with this object as its View - so the
    /// free-area search passes it by from now on and a press on any of its cells finds it. The object's sprite is
    /// fitted to the footprint and its health bar laid along the top of it.
    /// </summary>
    internal class PlaceBoardBuildingCommand : Command<BuildingPlacementVO>
    {
        /// <summary>The key of the building in CD_PoolGroup_BoardBuildings.</summary>
        private const string BuildingPoolKey = "board_building";

        /// <summary>How much of the footprint's width the health bar spans.</summary>
        private const float HealthBarWidth = 0.8f;

        /// <summary>How far below the footprint's top edge the health bar sits, in world units.</summary>
        private const float HealthBarInset = 0.2f;

        [Inject] private IBuildingsModel _buildingsModel { get; set; }
        [Inject] private IGridService    _gridService    { get; set; }
        [Inject] private IPoolService    _poolService    { get; set; }

        public override void Execute(BuildingPlacementVO placement)
        {
            BuildingCVO config = _buildingsModel.Buildings[placement.Type];

            var view = _poolService.Get<BoardBuilding>(BuildingPoolKey, _buildingsModel.BoardParent);
            _gridService.Occupy(placement.Area, new BoardBuildingVO(placement.Type, placement.Area, config.Hp, view));

            Rect area = _gridService.AreaToWorldRect(placement.Area);
            view.name = placement.Type.ToString();
            view.Sprite.Show(config.BoardSprite, area);

            // The object is scaled to the footprint, so the bar undoes that scale to keep its own size.
            Vector3 scale = view.transform.lossyScale;
            view.HealthBar.position = new Vector3(area.center.x, area.yMax - HealthBarInset, view.transform.position.z);
            view.HealthBar.localScale = new Vector3(area.width * HealthBarWidth / scale.x, 1f / scale.y, 1f);
        }
    }
}
