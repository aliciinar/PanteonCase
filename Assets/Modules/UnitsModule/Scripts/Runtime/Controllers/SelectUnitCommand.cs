using FlowIoC.BaseModule.Controller;
using FlowIoC.BaseModule.Function.Provider;
using FlowIoC.BaseModule.Injectable.Attributes;
using Modules.GridModule.Shared.Data.ValueObjects;
using Modules.UnitsModule.Controllers.BoardUnits;
using Modules.UnitsModule.Entities;
using Modules.UnitsModule.Models;
using UnityEngine;

namespace Modules.UnitsModule.Controllers
{
    /// <summary>
    /// A unit on the board was pressed: it becomes the selected one and its object - reached through its data - wears
    /// the selection tint; the one selected before, if any, is no longer and wears white. Pressing the selected unit
    /// again changes nothing.
    /// </summary>
    internal class SelectUnitCommand : Command
    {
        [Inject]      private IUnitSelectionModel _selectionModel   { get; set; }
        [Inject]      private IUnitsModel         _unitsModel       { get; set; }
        [Inject]      private IFunctionProvider   _functionProvider { get; set; }
        [SignalParam] private BoardUnitVO         _unit             { get; set; }

        public override void Execute()
        {
            BoardUnitVO previous = _selectionModel.Selected;
            if (previous == _unit) return;

            if (previous != null)
                _functionProvider.Call<TintBoardUnitFunction>().AddParams((BoardUnit)previous.View, Color.white).Execute();

            _selectionModel.Select(_unit);
            _functionProvider.Call<TintBoardUnitFunction>().AddParams((BoardUnit)_unit.View, _unitsModel.SelectedTint).Execute();
        }
    }
}
