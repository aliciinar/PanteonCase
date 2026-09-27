using FlowIoC.BaseModule.Controller;
using FlowIoC.BaseModule.Injectable.Attributes;
using Modules.BuildingsModule.Data.ValueObjects;
using Modules.BuildingsModule.Models;
using Modules.GridModule.Services;
using Modules.GridModule.Shared.Data.ValueObjects;

namespace Modules.BuildingsModule.Controllers
{
    /// <summary>
    /// Puts the placed building on the grid at full health: every cell of its area holds the same
    /// BoardBuildingVO, so the free-area search passes it by from now on and a press on any of its cells
    /// finds it. The grid is the one place the building's data is kept. The placement arrives from the
    /// step before it.
    /// </summary>
    internal class OccupyBuildingAreaCommand : Command<BuildingPlacementVO>
    {
        [Inject] private IGridService    _gridService    { get; set; }
        [Inject] private IBuildingsModel _buildingsModel { get; set; }

        public override void Execute(BuildingPlacementVO placement)
        {
            int hp = _buildingsModel.Buildings[placement.Type].Hp;
            _gridService.Occupy(placement.Area, new BoardBuildingVO(placement.Type, placement.Area, hp));
        }
    }
}
