using FlowIoC.BaseModule.Controller;
using FlowIoC.BaseModule.Injectable.Attributes;
using Modules.GridModule.Shared.Data.ValueObjects;
using Modules.UnitsModule.Models;
using Modules.UnitsModule.Signals;

namespace Modules.UnitsModule.Controllers
{
    /// <summary>
    /// A unit on the board was pressed: it becomes the selected one and is tinted; the one selected before, if any,
    /// is no longer. Pressing the selected unit again changes nothing.
    /// </summary>
    internal class SelectUnitCommand : Command
    {
        [Inject]       private IUnitSelectionModel  _selectionModel  { get; set; }
        [Inject]       private IUnitsModel          _unitsModel      { get; set; }
        [InjectSignal] private UnitsInternalSignals _internalSignals { get; set; }
        [SignalParam]  private BoardUnitVO          _unit            { get; set; }

        public override void Execute()
        {
            BoardUnitVO previous = _selectionModel.Selected;
            if (previous == _unit) return;

            if (previous != null) _internalSignals.HideUnitSelected.Dispatch(previous);

            _selectionModel.Select(_unit);
            _internalSignals.ShowUnitSelected.Dispatch(_unit, _unitsModel.SelectedTint);
        }
    }
}
