using FlowIoC.BaseModule.Function.ReturnableFunctions;
using UnityEngine;

namespace Modules.InputModule.Controllers
{
    /// <summary>
    /// The world point under a screen position, through the main camera - the game camera is the one
    /// tagged MainCamera. Correct for a camera that renders into part of the screen, since the camera
    /// converts through its own pixel rect.
    /// </summary>
    internal class ScreenToWorldFunction : FunctionReturn<Vector2, Vector2>
    {
        public override Vector2 Execute(Vector2 screenPosition) => Camera.main.ScreenToWorldPoint(screenPosition);
    }
}
