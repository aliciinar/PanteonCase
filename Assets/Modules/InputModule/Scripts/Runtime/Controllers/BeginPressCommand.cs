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
    /// the UI: only that it happened is announced, nothing of where, and no drag or release of it follows.
    /// On the world it is announced in world units. While an action runs (RD_GameStatus) nothing of the
    /// press is announced, wherever it is.
    /// </summary>
    internal class BeginPressCommand : Command
    {
        [Inject]       private IPointerModel     _pointerModel     { get; set; }
        [Inject]       private IFunctionProvider _functionProvider { get; set; }
        [InjectSignal] private InputSignals      _signals          { get; set; }
        [SignalParam]  private Vector2           _screenPosition   { get; set; }

        public override void Execute()
        {
            if (_pointerModel.IsGameLocked)
            {
                // Not on the world: none of this press - drag or release - is announced either.
                _pointerModel.BeginPress(false);
                return;
            }

            _pointerModel.BeginPress(!EventSystem.current.IsPointerOverGameObject());

            if (!_pointerModel.IsPressOnWorld)
            {
                _signals.Outgoing.PointerPressedOverUI.Dispatch();
                return;
            }

            Vector2 world = _functionProvider.Call<ScreenToWorldFunction>().AddParams(_screenPosition)
                                             .ExecuteAndGetResult<Vector2>();
            _signals.Outgoing.PointerPressed.Dispatch(world);
        }
    }
}
