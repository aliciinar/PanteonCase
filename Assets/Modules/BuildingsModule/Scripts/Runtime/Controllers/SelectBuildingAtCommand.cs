using FlowIoC.BaseModule.Controller;
using FlowIoC.BaseModule.Injectable.Attributes;
using Modules.BuildingsModule.Data.ValueObjects;
using Modules.BuildingsModule.Models;
using Modules.BuildingsModule.Shared.Data.ValueObjects;
using Modules.BuildingsModule.Signals;
using Modules.GridModule.Data.ValueObjects;
using Modules.GridModule.Enums;
using Modules.GridModule.Services;
using UnityEngine;

namespace Modules.BuildingsModule.Controllers
{
    /// <summary>
    /// Selects the building on the pressed cell and announces it - pressing the selected one again
    /// announces it again. A press anywhere else - a free cell, a unit, off the grid - clears the
    /// selection, and says so only when there was one to clear.
    /// </summary>
    internal class SelectBuildingAtCommand : Command
    {
        [Inject]       private IGridService         _gridService         { get; set; }
        [Inject]       private IBoardBuildingsModel _boardBuildingsModel { get; set; }
        [InjectSignal] private BuildingsSignals     _signals             { get; set; }
        [SignalParam]  private Vector2              _pointer             { get; set; }

        public override void Execute()
        {
            Vector2Int cell = _gridService.WorldToCell(_pointer);
            CellOccupantVO occupant = _gridService.IsInside(cell) ? _gridService.Cells[cell.x, cell.y].Occupant : null;

            if (occupant is { Type: CellOccupantType.Building })
            {
                BuildingRecordVO building = _boardBuildingsModel.Get(occupant.EntityId);
                _boardBuildingsModel.Select(building);
                _signals.Outgoing.BuildingSelected.Dispatch(new BuildingInfoVO(building.Type, building.Hp));
                return;
            }

            if (_boardBuildingsModel.ClearSelection()) _signals.Outgoing.SelectionCleared.Dispatch();
        }
    }
}
