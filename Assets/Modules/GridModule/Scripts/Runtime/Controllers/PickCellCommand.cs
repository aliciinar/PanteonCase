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
    /// Looks at the cell a press landed on and announces what stands there - a building or a unit, with
    /// the data the grid holds for it - or that nothing does, which is also the answer off the grid. Nothing is
    /// announced while a building is being placed: then a press only moves that building.
    /// </summary>
    internal class PickCellCommand : Command
    {
        [Inject]       private IGridModel   _gridModel   { get; set; }
        [Inject]       private IGridService _gridService { get; set; }
        [InjectSignal] private GridSignals  _signals     { get; set; }
        [SignalParam]  private Vector2      _pointer     { get; set; }

        public override void Execute()
        {
            if (!_gridModel.IsPicking) return;

            Vector2Int cell = _gridService.WorldToCell(_pointer);
            CellOccupantVO occupant = _gridService.IsInside(cell) ? _gridService.Cells[cell.x, cell.y].Occupant : null;

            switch (occupant)
            {
                case BoardBuildingVO building:
                    _signals.Outgoing.BuildingPressed.Dispatch(building);
                    break;
                case BoardUnitVO unit:
                    _signals.Outgoing.UnitPressed.Dispatch(unit);
                    break;
                default:
                    _signals.Outgoing.EmptyPressed.Dispatch();
                    break;
            }
        }
    }
}
