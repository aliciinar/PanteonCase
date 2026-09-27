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
    /// Something other than a building was pressed: the building that was selected, if any, wears white again and no
    /// building is selected. That nothing is selected is announced either way - the info screen hides only when open.
    /// </summary>
    internal class ClearBuildingSelectionCommand : Command
    {
        [Inject]       private IBuildingSelectionModel _selectionModel   { get; set; }
        [Inject]       private IFunctionProvider       _functionProvider { get; set; }
        [InjectSignal] private BuildingsSignals        _signals          { get; set; }

        public override void Execute()
        {
            BoardBuildingVO previous = _selectionModel.Selected;
            if (previous != null)
            {
                _selectionModel.ClearSelection();
                _functionProvider.Call<TintBoardBuildingFunction>().AddParams((BoardBuilding)previous.View, Color.white).Execute();
            }

            _signals.Outgoing.SelectionCleared.Dispatch();
        }
    }
}
