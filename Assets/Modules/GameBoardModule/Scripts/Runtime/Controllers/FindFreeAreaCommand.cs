using FlowIoC.BaseModule.Controller;
using FlowIoC.BaseModule.Injectable.Attributes;
using FlowIoC.ConsoleModule;
using Modules.GameBoardModule.Signals;
using Modules.GridModule.Services;
using UnityEngine;

namespace Modules.GameBoardModule.Controllers
{
    /// <summary>
    /// Finds where an area of the given size fits on the board, as close to the board's centre as
    /// possible (IGridService.FindFreeArea), and announces it - or announces that it fits nowhere and
    /// stops the flow.
    /// </summary>
    internal class FindFreeAreaCommand : Command
    {
        [Inject]       private IGridService     _gridService { get; set; }
        [InjectSignal] private GameBoardSignals _signals     { get; set; }
        [SignalParam]  private Vector2Int       _size        { get; set; }

        public override void Execute()
        {
            Retain();

            Vector2Int? origin = _gridService.FindFreeArea(_size);

            if (origin == null)
            {
                FlowLogger.Log($"FindFreeAreaCommand - no free {_size.x}x{_size.y} area left on the board.");
                _signals.Outgoing.NoFreeArea.Dispatch(_size);
                Stop();
                return;
            }

            _signals.Outgoing.FreeAreaFound.Dispatch(origin.Value, _size);
            Release();
        }
    }
}
