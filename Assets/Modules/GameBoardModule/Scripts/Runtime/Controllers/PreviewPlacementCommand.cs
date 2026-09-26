using FlowIoC.BaseModule.Controller;
using FlowIoC.BaseModule.Function.Provider;
using FlowIoC.BaseModule.Injectable.Attributes;
using Modules.GameBoardModule.Data.ValueObjects;
using Modules.GameBoardModule.Models;
using Modules.GameBoardModule.Signals;
using UnityEngine;

namespace Modules.GameBoardModule.Controllers
{
    /// <summary>
    /// Shows a placement as a preview and keeps it waiting for the player's answer. The preview is the
    /// building's ghost over its area with the confirm / cancel prompt beside it: centred one row above
    /// the area, or one row below when the area already reaches the board's top row and the prompt would
    /// leave the board. A new placement replaces whatever was waiting.
    /// </summary>
    internal class PreviewPlacementCommand : Command<BuildingPlacementVO>
    {
        [Inject]       private IGameBoardModel          _gameBoardModel   { get; set; }
        [Inject]       private IFunctionProvider        _functionProvider { get; set; }
        [InjectSignal] private GameBoardInternalSignals _internalSignals  { get; set; }

        public override void Execute(BuildingPlacementVO placement)
        {
            _gameBoardModel.PendingPlacement = placement;

            Rect area = _functionProvider.Call<AreaToWorldRectFunction>().AddParams(placement.Area)
                                         .ExecuteAndGetResult<Rect>();

            float halfCell = _gameBoardModel.CellSize * 0.5f;
            bool fitsAbove = placement.Area.yMax < _gameBoardModel.GridSize.y;
            var promptCentre = new Vector2(area.center.x, fitsAbove ? area.yMax + halfCell : area.yMin - halfCell);

            Sprite sprite = _gameBoardModel.Buildings[placement.Type].Sprite;
            _internalSignals.ShowPlacementPreview.Dispatch(new PlacementPreviewVO(sprite, area, promptCentre));
        }
    }
}
