using FlowIoC.BaseModule.Function.ReturnableFunctions;
using FlowIoC.BaseModule.Injectable.Attributes;
using Modules.GameBoardModule.Models;
using UnityEngine;

namespace Modules.GameBoardModule.Controllers
{
    /// <summary>The world rect an area of cells covers. Everything drawn over cells is placed with it.</summary>
    internal class AreaToWorldRectFunction : FunctionReturn<Rect, RectInt>
    {
        [Inject] private IGameBoardModel _gameBoardModel { get; set; }

        public override Rect Execute(RectInt area)
        {
            float cellSize = _gameBoardModel.CellSize;
            return new Rect(_gameBoardModel.GridBounds.min + (Vector2)area.position * cellSize,
                            (Vector2)area.size * cellSize);
        }
    }
}
