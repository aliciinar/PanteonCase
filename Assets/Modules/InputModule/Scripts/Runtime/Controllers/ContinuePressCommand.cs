using FlowIoC.BaseModule.Controller;
using FlowIoC.BaseModule.Function.Provider;
using FlowIoC.BaseModule.Injectable.Attributes;
using Modules.InputModule.Models;
using Modules.InputModule.Signals;
using UnityEngine;

namespace Modules.InputModule.Controllers
{
    /// <summary>
    /// The pointer moved while pressed. Announced only for a press that began on the world - which
    /// goes on being announced even when the pointer crosses UI on the way.
    /// </summary>
    internal class ContinuePressCommand : Command
    {
        [Inject]       private IPointerModel     _pointerModel     { get; set; }
        [Inject]       private IFunctionProvider _functionProvider { get; set; }
        [InjectSignal] private InputSignals      _signals          { get; set; }
        [SignalParam]  private Vector2           _screenPosition   { get; set; }

        public override void Execute()
        {
            if (!_pointerModel.IsPressOnWorld) return;

            Vector2 world = _functionProvider.Call<ScreenToWorldFunction>().AddParams(_screenPosition)
                                             .ExecuteAndGetResult<Vector2>();
            _signals.Outgoing.PointerDragged.Dispatch(world);
        }
    }
}
