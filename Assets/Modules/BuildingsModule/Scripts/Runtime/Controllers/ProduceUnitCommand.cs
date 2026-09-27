using FlowIoC.BaseModule.Controller;
using FlowIoC.BaseModule.Injectable.Attributes;
using Modules.BuildingsModule.Data.ValueObjects;
using Modules.BuildingsModule.Models;
using Modules.BuildingsModule.Shared.Data.ValueObjects;
using Modules.BuildingsModule.Signals;
using Modules.UnitsModule.Shared.Data.ValueObjects;
using Modules.UnitsModule.Shared.Enums;

namespace Modules.BuildingsModule.Controllers
{
    /// <summary>
    /// The selected building asks for a unit of this type: it comes out of the building's exit point
    /// (its door) and walks to its spawn point, both turned from the building's own cells into the
    /// grid's. Where it actually appears and how it gets there is the units module's business.
    /// </summary>
    internal class ProduceUnitCommand : Command
    {
        [Inject]       private IBuildingsModel      _buildingsModel      { get; set; }
        [Inject]       private IBoardBuildingsModel _boardBuildingsModel { get; set; }
        [InjectSignal] private BuildingsSignals     _signals             { get; set; }
        [SignalParam]  private UnitType             _unit                { get; set; }

        public override void Execute()
        {
            BuildingRecordVO producer = _boardBuildingsModel.Selected;
            BuildingCVO building = _buildingsModel.Buildings[producer.Type];

            _signals.Outgoing.UnitRequested.Dispatch(new UnitSpawnRequestVO(_unit,
                                                                            producer.Area.position + building.ExitPoint,
                                                                            producer.Area.position + building.SpawnPoint));
        }
    }
}
