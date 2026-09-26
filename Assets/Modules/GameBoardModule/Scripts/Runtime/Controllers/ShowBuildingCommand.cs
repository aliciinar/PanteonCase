using FlowIoC.BaseModule.Controller;
using FlowIoC.BaseModule.Function.Provider;
using FlowIoC.BaseModule.Injectable.Attributes;
using FlowIoC.PoolModule.Services;
using Modules.GameBoardModule.Data.ValueObjects;
using Modules.GameBoardModule.Entities;
using Modules.GameBoardModule.Models;
using Modules.GameBoardModule.Signals;
using UnityEngine;

namespace Modules.GameBoardModule.Controllers
{
    /// <summary>
    /// Shows a placed building over its area. The placement arrives from the step before it; its cells
    /// are turned into the world rect they cover, a building comes out of the pool (group "gameboard",
    /// warmed while the game loaded) and the board view puts it there.
    /// </summary>
    internal class ShowBuildingCommand : Command<BuildingPlacementVO>
    {
        /// <summary>The key of the building in CD_PoolGroup_GameBoard.</summary>
        private const string BuildingPoolKey = "board_building";

        [Inject]       private IGameBoardModel          _gameBoardModel   { get; set; }
        [Inject]       private IFunctionProvider        _functionProvider { get; set; }
        [Inject]       private IPoolService             _poolService      { get; set; }
        [InjectSignal] private GameBoardInternalSignals _internalSignals  { get; set; }

        public override void Execute(BuildingPlacementVO placement)
        {
            Rect area = _functionProvider.Call<AreaToWorldRectFunction>().AddParams(placement.Area)
                                         .ExecuteAndGetResult<Rect>();

            var building = _poolService.Get<BoardBuilding>(BuildingPoolKey, null);
            Sprite sprite = _gameBoardModel.Buildings[placement.Type].Sprite;

            _internalSignals.ShowBuilding.Dispatch(new BoardBuildingVO(building, placement.Type, sprite, area));
        }
    }
}
