using FlowIoC.BaseModule.Controller;
using FlowIoC.BaseModule.Function.Provider;
using FlowIoC.BaseModule.Injectable.Attributes;
using Modules.BuildingsModule.Controllers.BoardBuildings;
using Modules.BuildingsModule.Entities;
using Modules.BuildingsModule.Models;
using Modules.BuildingsModule.Shared.Data.ValueObjects;
using Modules.BuildingsModule.Signals;
using Modules.GridModule.Shared.Data.ValueObjects;
using UnityEngine;

namespace Modules.BuildingsModule.Controllers
{
    /// <summary>
    /// A building on the board was pressed: it becomes the selected one and its object - reached through its data -
    /// wears the selection colour; the one selected before, if any, wears white again. It is announced as selected -
    /// pressing it again announces it again - with its door and spawn point turned from the building's own cells into
    /// the grid's, so whoever shows it can ask it for a unit without coming back here.
    /// </summary>
    internal class SelectBuildingCommand : Command
    {
        [Inject]       private IBuildingsModel         _buildingsModel   { get; set; }
        [Inject]       private IBuildingSelectionModel _selectionModel   { get; set; }
        [Inject]       private IFunctionProvider       _functionProvider { get; set; }
        [InjectSignal] private BuildingsSignals        _signals          { get; set; }
        [SignalParam]  private BoardBuildingVO         _building         { get; set; }

        public override void Execute()
        {
            BoardBuildingVO previous = _selectionModel.Selected;
            if (previous != _building)
            {
                if (previous != null)
                    _functionProvider.Call<ColorBoardBuildingFunction>().AddParams((BoardBuilding)previous.View, Color.white).Execute();

                _selectionModel.Select(_building);
                _functionProvider.Call<ColorBoardBuildingFunction>()
                                 .AddParams((BoardBuilding)_building.View, _buildingsModel.SelectedColor)
                                 .Execute();
            }

            BuildingCVO config = _buildingsModel.Buildings[_building.Type];
            _signals.Outgoing.BuildingSelected.Dispatch(new BuildingInfoVO(_building.Type,
                                                                          _building.Hp,
                                                                          _building.Area.position + config.ExitPoint,
                                                                          _building.Area.position + config.SpawnPoint));
        }
    }
}
