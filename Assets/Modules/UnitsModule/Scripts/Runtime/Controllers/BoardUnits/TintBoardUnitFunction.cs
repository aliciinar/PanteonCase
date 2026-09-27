using DG.Tweening;
using FlowIoC.BaseModule.Function.VoidFunctions;
using Modules.UnitsModule.Entities;
using UnityEngine;

namespace Modules.UnitsModule.Controllers.BoardUnits
{
    /// <summary>Dresses a unit in a colour - the selection tint, or white - ending any hit flash, which would hand back the old one.</summary>
    internal class TintBoardUnitFunction : FunctionVoid<BoardUnit, Color>
    {
        public override void Execute(BoardUnit unit, Color tint)
        {
            unit.Flash?.Kill();
            unit.Tint = tint;
            unit.Renderer.color = tint;
        }
    }
}
