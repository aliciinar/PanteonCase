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
    /// Finds the free area nearest the board's centre that a building's footprint fits in, and hands the
    /// building and that area to the next step as a BuildingPlacementVO. If the footprint fits nowhere,
    /// announces NoFreeArea and stops the flow.
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
                FlowLogger.Log($"FindBuildingAreaCommand - no free {size.x}x{size.y} area left for {_buildType}.");
                _signals.Outgoing.NoFreeArea.Dispatch(size);
                Stop();
                return;
            }

            Release(new BuildingPlacementVO(_buildType, new RectInt(origin.Value, size)));
        }
    }
}
