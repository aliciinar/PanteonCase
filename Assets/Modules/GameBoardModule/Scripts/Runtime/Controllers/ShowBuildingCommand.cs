using FlowIoC.BaseModule.Controller;
using FlowIoC.BaseModule.Injectable.Attributes;
using FlowIoC.PoolModule.Services;
using Modules.BuildingsModule.Shared.Enums;
using Modules.GameBoardModule.Data.ValueObjects;
using Modules.GameBoardModule.Entities;
using Modules.GameBoardModule.Models;
using Modules.GameBoardModule.Signals;
using UnityEngine;

namespace Modules.GameBoardModule.Controllers
{
    /// <summary>
    /// Shows a building over an area of the board. The area arrives from the step before it, in cells;
    /// it is turned into the world rect those cells cover, a building comes out of the pool (group
    /// "gameboard", warmed while the game loaded) and the board view puts it there.
    /// </summary>
    internal class ShowBuildingCommand : Command<RectInt>
    {
        /// <summary>The key of the building in CD_PoolGroup_GameBoard.</summary>
        private const string BuildingPoolKey = "board_building";

        [Inject]       private IGameBoardModel          _gameBoardModel  { get; set; }
        [Inject]       private IPoolService             _poolService     { get; set; }
        [InjectSignal] private GameBoardInternalSignals _internalSignals { get; set; }
        [SignalParam]  private BuildType                _buildType       { get; set; }

        public override void Execute(RectInt area)
        {
            float cellSize = _gameBoardModel.CellSize;
            var worldArea = new Rect(_gameBoardModel.GridBounds.min + (Vector2)area.position * cellSize,
                                     (Vector2)area.size * cellSize);

            var building = _poolService.Get<BoardBuilding>(BuildingPoolKey, null);
            Sprite sprite = _gameBoardModel.Buildings[_buildType].Sprite;

            _internalSignals.ShowBuilding.Dispatch(new BoardBuildingVO(building, _buildType, sprite, worldArea));
        }
    }
}
