using FlowIoC.BaseModule.Controller;
using FlowIoC.BaseModule.Injectable.Attributes;
using Modules.GridModule.Shared.Data.ValueObjects;
using Modules.UnitsModule.Models;
using Modules.UnitsModule.Signals;

namespace Modules.UnitsModule.Controllers
{
    /// <summary>Something other than a unit was pressed: no unit is selected any more, and the one that was loses its tint.</summary>
    internal class ClearUnitSelectionCommand : Command
    {
        [Inject]       private IUnitSelectionModel  _selectionModel  { get; set; }
        [InjectSignal] private UnitsInternalSignals _internalSignals { get; set; }

        public override void Execute()
        {
            BoardUnitVO previous = _selectionModel.Selected;
            if (previous == null) return;

            _selectionModel.ClearSelection();
            _internalSignals.HideUnitSelected.Dispatch(previous);
        }
    }
}
