using FlowIoC.BaseModule.Controller;
using FlowIoC.BaseModule.Injectable.Attributes;
using Modules.MainModule.Models;
using Modules.MainModule.Signals;
using UnityEngine;

namespace Modules.MainModule.Controllers
{
    /// <summary>
    /// Takes a size the window reported, stores it in ScreenResizeModel and announces it on
    /// MainSignals.Outgoing.ScreenResized - unless nothing changed, or the window was minimised and
    /// reports a zero-sized screen there is nothing to lay out against.
    /// </summary>
    internal class ApplyScreenSizeCommand : Command
    {
        [Inject]       private IScreenResizeModel _screenResizeModel { get; set; }
        [InjectSignal] private MainSignals        _mainSignals       { get; set; }
        [SignalParam]  private Vector2Int         _screenSize        { get; set; }

        public override void Execute()
        {
            if (_screenSize == _screenResizeModel.ScreenSize || _screenSize.x <= 0 || _screenSize.y <= 0) return;

            _screenResizeModel.SetScreenSize(_screenSize);
            _mainSignals.Outgoing.ScreenResized.Dispatch(_screenSize);
        }
    }
}
