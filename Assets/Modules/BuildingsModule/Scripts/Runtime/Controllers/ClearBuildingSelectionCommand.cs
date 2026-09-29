using FlowIoC.BaseModule.Controller;
using FlowIoC.BaseModule.Function.Provider;
using FlowIoC.BaseModule.Injectable.Attributes;
using Modules.BuildingsModule.Controllers.BoardBuildings;
using Modules.BuildingsModule.Entities;
using Modules.BuildingsModule.Models;
using Modules.BuildingsModule.Signals;
using Modules.GridModule.Shared.Data.ValueObjects;
using UnityEngine;

namespace Modules.BuildingsModule.Controllers
{
    /// <summary>
    /// Something other than a building was pressed: no building is selected any more, the object of the one that was -
    /// reached through its data - wears white again, and the cleared selection is announced. With nothing selected
    /// nothing changes and nothing is announced.
    /// </summary>
    internal class ClearBuildingSelectionCommand : Command
    {
        [Inject]       private IBuildingSelectionModel _selectionModel   { get; set; }
        [Inject]       private IFunctionProvider       _functionProvider { get; set; }
        [InjectSignal] private BuildingsSignals        _signals          { get; set; }

        public override void Execute()
        {
            BoardBuildingVO previous = _selectionModel.Selected;
            if (previous == null) return;

            _selectionModel.ClearSelection();
            _functionProvider.Call<ColorBoardBuildingFunction>().AddParams((BoardBuilding)previous.View, Color.white).Execute();

            _signals.Outgoing.SelectionCleared.Dispatch();
        }
    }
}
