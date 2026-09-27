using DG.Tweening;
using FlowIoC.BaseModule.Function.VoidFunctions;
using Modules.BuildingsModule.Entities;
using UnityEngine;

namespace Modules.BuildingsModule.Controllers.BoardBuildings
{
    /// <summary>Dresses a building in a colour - the selection tint, or white - ending any hit flash, which would hand back the old one.</summary>
    internal class TintBoardBuildingFunction : FunctionVoid<BoardBuilding, Color>
    {
        public override void Execute(BoardBuilding building, Color tint)
        {
            building.Flash?.Kill();
            building.Sprite.Tint(tint);
        }
    }
}
