using FlowIoC.BaseModule.Controller;
using FlowIoC.BaseModule.Injectable.Attributes;
using Modules.BuildingsModule.Data.ValueObjects;
using Modules.BuildingsModule.Models;
using Modules.BuildingsModule.Signals;
using Modules.GridModule.Services;
using UnityEngine;

namespace Modules.BuildingsModule.Controllers
{
    /// <summary>
    /// Shows a placement as a preview and keeps it waiting for the player's answer. The preview is the
    /// building's ghost over its area - green where it fits, red where it does not, and only a fitting
    /// one can be confirmed - with the confirm / cancel prompt beside it: centred one row above the
    /// area, or one row below when the area already reaches the board's top row and the prompt would
    /// leave the board. A new placement replaces whatever was waiting.
    /// </summary>
    internal class PreviewPlacementCommand : Command<BuildingPlacementVO>
    {
        [Inject]       private IBuildingsModel          _buildingsModel  { get; set; }
        [Inject]       private IPlacementModel          _placementModel  { get; set; }
        [Inject]       private IGridService             _gridService     { get; set; }
        [InjectSignal] private BuildingsInternalSignals _internalSignals { get; set; }

        public override void Execute(BuildingPlacementVO placement)
        {
            _placementModel.Wait(placement);

            Rect area = _gridService.AreaToWorldRect(placement.Area);
            bool fits = _gridService.IsAreaFree(placement.Area);

            float cellSize = _gridService.CellSize;
            float halfCell = cellSize * 0.5f;
            bool fitsAbove = placement.Area.yMax < _gridService.GridSize.y;
            var promptCentre = new Vector2(area.center.x, fitsAbove ? area.yMax + halfCell : area.yMin - halfCell);

            Sprite sprite = _buildingsModel.Buildings[placement.Type].BoardSprite;
            _internalSignals.ShowPlacementPreview.Dispatch(new PlacementPreviewVO(sprite, area, promptCentre, fits, cellSize));
        }
    }
}
