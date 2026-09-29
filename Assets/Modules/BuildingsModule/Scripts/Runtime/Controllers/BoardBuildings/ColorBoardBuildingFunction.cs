using DG.Tweening;
using FlowIoC.BaseModule.Function.VoidFunctions;
using Modules.BuildingsModule.Entities;
using UnityEngine;

namespace Modules.BuildingsModule.Controllers.BoardBuildings
{
    /// <summary>Dresses a building in a colour - the selection colour, or white - ending any hit flash, which would hand back the old one.</summary>
    internal class ColorBoardBuildingFunction : FunctionVoid<BoardBuilding, Color>
    {
        public override void Execute(BoardBuilding building, Color color)
        {
            building.Flash?.Kill();
            building.Sprite.SetColor(color);
        }
    }
}
