using FlowIoC.BaseModule.Controller;
using FlowIoC.BaseModule.Injectable.Attributes;
using Modules.GridModule.Models;
using Modules.GridModule.Services;
using Modules.GridModule.Shared.Data.ValueObjects;
using Modules.GridModule.Signals;
using UnityEngine;

namespace Modules.GridModule.Controllers
{
    /// <summary>
    /// Looks at the cell a secondary press landed on: a free cell of the grid is announced as an order to go there,
    /// a building or a unit as an order to attack it, with the data the grid holds for it. A secondary press off the
    /// grid is not answered, nor one while a building is being placed.
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
            if (!_gridService.IsInside(cell)) return;

            CellOccupantVO occupant = _gridService.Cells[cell.x, cell.y].Occupant;

            if (occupant == null)
                _signals.Outgoing.FreeCellSecondaryPressed.Dispatch(cell);
            else
                _signals.Outgoing.OccupantSecondaryPressed.Dispatch(occupant);
        }
    }
}
