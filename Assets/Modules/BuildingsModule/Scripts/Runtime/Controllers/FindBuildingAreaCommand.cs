using FlowIoC.BaseModule.Controller;
using FlowIoC.BaseModule.Injectable.Attributes;
using FlowIoC.ConsoleModule;
using Modules.BuildingsModule.Data.ValueObjects;
using Modules.BuildingsModule.Models;
using Modules.BuildingsModule.Shared.Enums;
using Modules.GridModule.Services;
using UnityEngine;

namespace Modules.BuildingsModule.Controllers
{
    /// <summary>
    /// Finds where a picked building starts: the free area nearest the board's centre that its
    /// footprint fits in. When it fits nowhere, it starts centred on the board anyway - shown red, for
    /// the player to move. The building and its area go to the next step as a BuildingPlacementVO.
    /// </summary>
    internal class FindBuildingAreaCommand : Command
    {
        [Inject]      private IBuildingsModel _buildingsModel { get; set; }
        [Inject]      private IGridService    _gridService    { get; set; }
        [SignalParam] private BuildType       _buildType      { get; set; }

        public override void Execute()
        {
            Retain();

            Vector2Int size = _buildingsModel.Buildings[_buildType].Size;
            Vector2Int? origin = _gridService.FindNearestFreeAreaBfs(size);

            if (origin == null)
            {
                FlowLogger.Log($"FindBuildingAreaCommand - no free {size.x}x{size.y} area left for {_buildType}; it starts at the centre.");
                origin = _gridService.CentredOrigin(size);
            }

            Release(new BuildingPlacementVO(_buildType, new RectInt(origin.Value, size)));
        }
    }
}
