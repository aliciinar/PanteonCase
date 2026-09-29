using FlowIoC.BaseModule.Controller;
using FlowIoC.BaseModule.Function.Provider;
using FlowIoC.BaseModule.Injectable.Attributes;
using Modules.GridModule.Shared.Data.ValueObjects;
using Modules.UnitsModule.Controllers.BoardUnits;
using Modules.UnitsModule.Entities;
using Modules.UnitsModule.Models;
using Modules.UnitsModule.Shared.Data.ValueObjects;
using Modules.UnitsModule.Signals;
using UnityEngine;

namespace Modules.UnitsModule.Controllers
{
    /// <summary>
    /// A unit on the board was pressed: it becomes the selected one, its object - reached through its data - wears
    /// the selection colour, and the selection is announced with the unit's health now; the one selected before, if
    /// any, is no longer and wears white. Pressing the selected unit again announces it again, so whoever shows it
    /// shows its health as it is now.
    /// </summary>
    internal class SelectUnitCommand : Command
    {
        [Inject]       private IUnitSelectionModel _selectionModel   { get; set; }
        [Inject]       private IUnitsModel         _unitsModel       { get; set; }
        [Inject]       private IFunctionProvider   _functionProvider { get; set; }
        [InjectSignal] private UnitsSignals        _signals          { get; set; }
        [SignalParam]  private BoardUnitVO         _unit             { get; set; }

        public override void Execute()
        {
            BoardUnitVO previous = _selectionModel.Selected;
            if (previous != _unit)
            {
                if (previous != null)
                    _functionProvider.Call<ColorBoardUnitFunction>().AddParams((BoardUnit)previous.View, Color.white).Execute();

                _selectionModel.Select(_unit);
                _functionProvider.Call<ColorBoardUnitFunction>().AddParams((BoardUnit)_unit.View, _unitsModel.SelectedColor).Execute();
            }

            _signals.Outgoing.UnitSelected.Dispatch(new UnitInfoVO(_unit.Type, _unit.Hp));
        }
    }
}
