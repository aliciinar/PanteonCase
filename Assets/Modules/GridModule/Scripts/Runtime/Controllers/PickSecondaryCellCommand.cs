using FlowIoC.BaseModule.Controller;
using FlowIoC.BaseModule.Injectable.Attributes;
using Modules.GridModule.Models;
using Modules.GridModule.Services;
using Modules.GridModule.Signals;
using UnityEngine;

namespace Modules.GridModule.Controllers
{
    /// <summary>
    /// Looks at the cell a secondary press landed on and, when it is a free cell of the grid, announces it - an
    /// order to go there. A secondary press on a building or a unit is not answered yet, nor one off the grid, nor
    /// one while a building is being placed.
    /// </summary>
    internal class PickSecondaryCellCommand : Command
    {
        [Inject]       private IGridModel   _gridModel   { get; set; }
        [Inject]       private IGridService _gridService { get; set; }
        [InjectSignal] private GridSignals  _signals     { get; set; }
        [SignalParam]  private Vector2      _pointer     { get; set; }

        public override void Execute()
        {
            if (!_gridModel.IsPicking) return;

            Vector2Int cell = _gridService.WorldToCell(_pointer);
            if (!_gridService.IsInside(cell) || !_gridService.Cells[cell.x, cell.y].IsFree) return;

            _signals.Outgoing.FreeCellSecondaryPressed.Dispatch(cell);
        }
    }
}
