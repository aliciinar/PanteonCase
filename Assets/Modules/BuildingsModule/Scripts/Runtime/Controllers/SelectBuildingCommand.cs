using FlowIoC.BaseModule.Controller;
using FlowIoC.BaseModule.Injectable.Attributes;
using Modules.BuildingsModule.Models;
using Modules.BuildingsModule.Shared.Data.ValueObjects;
using Modules.BuildingsModule.Signals;
using Modules.GridModule.Shared.Data.ValueObjects;

namespace Modules.BuildingsModule.Controllers
{
    /// <summary>
    /// A building on the board was pressed: it is announced as selected - pressing it again announces it
    /// again - with its door and spawn point turned from the building's own cells into the grid's, so
    /// whoever shows it can ask it for a unit without coming back here. Nothing is kept: the building's
    /// data is the grid's, and the announcement carries what is needed. While a building waits to be
    /// placed a press only moves that building, so nothing is selected.
    /// </summary>
    internal class SelectBuildingCommand : Command
    {
        [Inject]       private IPlacementModel  _placementModel { get; set; }
        [Inject]       private IBuildingsModel  _buildingsModel { get; set; }
        [InjectSignal] private BuildingsSignals _signals        { get; set; }
        [SignalParam]  private BoardBuildingVO  _building       { get; set; }

        public override void Execute()
        {
            if (_placementModel.IsWaiting) return;

            BuildingCVO config = _buildingsModel.Buildings[_building.Type];
            _signals.Outgoing.BuildingSelected.Dispatch(new BuildingInfoVO(_building.Type,
                                                                          _building.Hp,
                                                                          _building.Area.position + config.ExitPoint,
                                                                          _building.Area.position + config.SpawnPoint));
        }
    }
}
