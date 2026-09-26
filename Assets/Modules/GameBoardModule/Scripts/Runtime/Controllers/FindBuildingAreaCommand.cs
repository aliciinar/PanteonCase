using FlowIoC.BaseModule.Controller;
using FlowIoC.BaseModule.Function.Provider;
using FlowIoC.BaseModule.Injectable.Attributes;
using FlowIoC.ConsoleModule;
using Modules.BuildingsModule.Shared.Enums;
using Modules.GameBoardModule.Data.ValueObjects;
using Modules.GameBoardModule.Models;
using Modules.GameBoardModule.Signals;
using UnityEngine;

namespace Modules.GameBoardModule.Controllers
{
    /// <summary>
    /// Finds where a picked building starts: the free area nearest the board's centre that its
    /// footprint fits in. When it fits nowhere, it starts centred on the board anyway - shown red, for
    /// the player to move - and NoFreeArea is announced. The building and its area go to the next step
    /// as a BuildingPlacementVO.
    /// </summary>
    internal class FindBuildingAreaCommand : Command
    {
        [Inject]       private IGameBoardModel   _gameBoardModel   { get; set; }
        [Inject]       private IFunctionProvider _functionProvider { get; set; }
        [InjectSignal] private GameBoardSignals  _signals          { get; set; }
        [SignalParam]  private BuildType         _buildType        { get; set; }

        public override void Execute()
        {
            Retain();

            Vector2Int size = _gameBoardModel.Buildings[_buildType].Size;
            Vector2Int? origin = _functionProvider.Call<FindFreeAreaFunction>().AddParams(size)
                                                  .ExecuteAndGetResult<Vector2Int?>();

            if (origin == null)
            {
                FlowLogger.Log($"FindBuildingAreaCommand - no free {size.x}x{size.y} area left for {_buildType}; it starts at the centre.");
                _signals.Outgoing.NoFreeArea.Dispatch(size);

                Vector2Int gridSize = _gameBoardModel.GridSize;
                origin = new Vector2Int(Mathf.Max(0, (gridSize.x - size.x) / 2), Mathf.Max(0, (gridSize.y - size.y) / 2));
            }

            Release(new BuildingPlacementVO(_buildType, new RectInt(origin.Value, size)));
        }
    }
}
