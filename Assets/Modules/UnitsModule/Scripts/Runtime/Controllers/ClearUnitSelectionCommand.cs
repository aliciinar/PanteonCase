using FlowIoC.BaseModule.Controller;
using FlowIoC.BaseModule.Function.Provider;
using FlowIoC.BaseModule.Injectable.Attributes;
using Modules.GridModule.Shared.Data.ValueObjects;
using Modules.UnitsModule.Controllers.BoardUnits;
using Modules.UnitsModule.Entities;
using Modules.UnitsModule.Models;
using Modules.UnitsModule.Signals;
using UnityEngine;

namespace Modules.UnitsModule.Controllers
{
    /// <summary>
    /// Something other than a unit was pressed: no unit is selected any more, the object of the one that was -
    /// reached through its data - wears white again, and the cleared selection is announced. With nothing selected
    /// nothing changes and nothing is announced.
    /// </summary>
    internal class ClearUnitSelectionCommand : Command
    {
        [Inject]       private IUnitSelectionModel _selectionModel   { get; set; }
        [Inject]       private IFunctionProvider   _functionProvider { get; set; }
        [InjectSignal] private UnitsSignals        _signals          { get; set; }

        public override void Execute()
        {
            BoardUnitVO previous = _selectionModel.Selected;
            if (previous == null) return;

            _selectionModel.ClearSelection();
            _functionProvider.Call<ColorBoardUnitFunction>().AddParams((BoardUnit)previous.View, Color.white).Execute();

            _signals.Outgoing.SelectionCleared.Dispatch();
        }
    }
}
