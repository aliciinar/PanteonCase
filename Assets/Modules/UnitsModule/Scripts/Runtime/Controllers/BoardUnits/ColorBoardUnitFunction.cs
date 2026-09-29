using DG.Tweening;
using FlowIoC.BaseModule.Function.VoidFunctions;
using Modules.UnitsModule.Entities;
using UnityEngine;

namespace Modules.UnitsModule.Controllers.BoardUnits
{
    /// <summary>Dresses a unit in a colour - the selection colour, or white - ending any hit flash, which would hand back the old one.</summary>
    internal class ColorBoardUnitFunction : FunctionVoid<BoardUnit, Color>
    {
        public override void Execute(BoardUnit unit, Color color)
        {
            unit.Flash?.Kill();
            unit.BaseColor = color;
            unit.Renderer.color = color;
        }
    }
}
