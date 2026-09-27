using FlowIoC.BaseModule.Controller;
using FlowIoC.BaseModule.Injectable.Attributes;
using Modules.BuildingsModule.Data.ValueObjects;
using Modules.BuildingsModule.Models;
using Modules.GridModule.Enums;
using Modules.GridModule.Services;

namespace Modules.BuildingsModule.Controllers
{
    /// <summary>
    /// Marks every cell of a placement's area as standing under a new building, so the free-area search
    /// passes it by from now on. The placement arrives from the step before it; the grid service gives
    /// the building an entity id of its own, which is what tells two neighbouring buildings' cells apart,
    /// and the building is recorded under that id at full health - a press on any of its cells finds it.
    /// </summary>
    internal class OccupyBuildingAreaCommand : Command<BuildingPlacementVO>
    {
        [Inject] private IGridService         _gridService         { get; set; }
        [Inject] private IBuildingsModel      _buildingsModel      { get; set; }
        [Inject] private IBoardBuildingsModel _boardBuildingsModel { get; set; }

        public override void Execute(BuildingPlacementVO placement)
        {
            int id = _gridService.Occupy(placement.Area, CellOccupantType.Building);
            int hp = _buildingsModel.Buildings[placement.Type].Hp;

            _boardBuildingsModel.Add(new BuildingRecordVO(id, placement.Type, placement.Area, hp));
        }
    }
}
