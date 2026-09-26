using FlowIoC.BaseModule.Controller;
using FlowIoC.BaseModule.Injectable.Attributes;
using Modules.BuildingsModule.Data.ValueObjects;
using Modules.GridModule.Services;
using Modules.GridModule.Shared.Enums;

namespace Modules.BuildingsModule.Controllers
{
    /// <summary>
    /// Marks every cell of a placement's area as standing under a new building, so the free-area search
    /// passes it by from now on. The placement arrives from the step before it; the grid service gives
    /// the building an entity id of its own, which is what tells two neighbouring buildings' cells apart.
    /// </summary>
    internal class OccupyBuildingAreaCommand : Command<BuildingPlacementVO>
    {
        [Inject] private IGridService _gridService { get; set; }

        public override void Execute(BuildingPlacementVO placement) =>
            _gridService.Occupy(placement.Area, CellOccupantType.Building);
    }
}
