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
    /// A press began. Over UI - a HUD panel, a production card, the placement prompt - it belongs to
    /// the UI and nothing of it is announced; on the world it is announced in world units.
    /// </summary>
    internal class BeginPressCommand : Command
    {
        [Inject]       private IPointerModel     _pointerModel     { get; set; }
        [Inject]       private IFunctionProvider _functionProvider { get; set; }
        [InjectSignal] private InputSignals      _signals          { get; set; }
        [SignalParam]  private Vector2           _screenPosition   { get; set; }

        public override void Execute()
        {
            _pointerModel.IsPressOnWorld = !EventSystem.current.IsPointerOverGameObject();
            if (!_pointerModel.IsPressOnWorld) return;

            Vector2 world = _functionProvider.Call<ScreenToWorldFunction>().AddParams(_screenPosition)
                                             .ExecuteAndGetResult<Vector2>();
            _signals.Outgoing.PointerPressed.Dispatch(world);
        }
    }
}
