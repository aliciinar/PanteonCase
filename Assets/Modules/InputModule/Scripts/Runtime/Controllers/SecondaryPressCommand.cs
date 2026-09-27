using FlowIoC.BaseModule.Controller;
using FlowIoC.BaseModule.Function.Provider;
using FlowIoC.BaseModule.Injectable.Attributes;
using Modules.InputModule.Models;
using Modules.InputModule.Signals;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Modules.InputModule.Controllers
{
    /// <summary>
    /// The secondary button went down. Over UI it belongs to the UI and is not announced; on the world it
    /// is announced in world units. A secondary press has no drag or release anyone needs. While an action
    /// runs (RD_GameStatus) it is not announced at all.
    /// </summary>
    internal class SecondaryPressCommand : Command
    {
        [Inject]       private IPointerModel     _pointerModel     { get; set; }
        [Inject]       private IFunctionProvider _functionProvider { get; set; }
        [InjectSignal] private InputSignals      _signals          { get; set; }
        [SignalParam]  private Vector2           _screenPosition   { get; set; }

        public override void Execute()
        {
            if (_pointerModel.IsGameLocked || EventSystem.current.IsPointerOverGameObject()) return;

            Vector2 world = _functionProvider.Call<ScreenToWorldFunction>().AddParams(_screenPosition)
                                             .ExecuteAndGetResult<Vector2>();
            _signals.Outgoing.PointerSecondaryPressed.Dispatch(world);
        }
    }
}
