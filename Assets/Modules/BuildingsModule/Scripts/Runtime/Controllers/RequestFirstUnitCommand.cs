using FlowIoC.BaseModule.Controller;
using FlowIoC.BaseModule.Injectable.Attributes;
using Modules.BuildingsModule.Data.ValueObjects;
using Modules.BuildingsModule.Models;
using Modules.BuildingsModule.Shared.Data.ValueObjects;
using Modules.BuildingsModule.Signals;
using Modules.UnitsModule.Shared.Data.ValueObjects;

namespace Modules.BuildingsModule.Controllers
{
    /// <summary>
    /// A placed building that produces units asks for its first one right away - the first unit it
    /// produces, coming out of its exit point (its door) and walking to its spawn point, both turned into
    /// the grid's cells. A stand-in until units are produced from the information panel; a building that
    /// produces nothing asks for nothing.
    /// </summary>
    internal class RequestFirstUnitCommand : Command<BuildingPlacementVO>
    {
        [Inject]       private IBuildingsModel  _buildingsModel { get; set; }
        [InjectSignal] private BuildingsSignals _signals        { get; set; }

        public override void Execute(BuildingPlacementVO placement)
        {
            BuildingCVO building = _buildingsModel.Buildings[placement.Type];
            if (building.ProducibleUnits.Count == 0) return;

            _signals.Outgoing.UnitRequested.Dispatch(new UnitSpawnRequestVO(building.ProducibleUnits[0],
                                                                            placement.Area.position + building.ExitPoint,
                                                                            placement.Area.position + building.SpawnPoint));
        }
    }
}
