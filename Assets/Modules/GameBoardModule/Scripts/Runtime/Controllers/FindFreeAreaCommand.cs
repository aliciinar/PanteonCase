using FlowIoC.BaseModule.Controller;
using FlowIoC.BaseModule.Function.Provider;
using FlowIoC.BaseModule.Injectable.Attributes;
using FlowIoC.ConsoleModule;
using Modules.GameBoardModule.Signals;
using UnityEngine;

namespace Modules.GameBoardModule.Controllers
{
    /// <summary>
    /// Finds where an area of the given size fits on the board, as close to the board's centre as
    /// possible (FindFreeAreaFunction), and announces it - or announces that it fits nowhere and stops
    /// the flow.
    /// </summary>
    internal class FindFreeAreaCommand : Command
    {
        [Inject]       private IFunctionProvider _functionProvider { get; set; }
        [InjectSignal] private GameBoardSignals  _signals          { get; set; }
        [SignalParam]  private Vector2Int        _size             { get; set; }

        public override void Execute()
        {
            Retain();

            Vector2Int? origin = _functionProvider.Call<FindFreeAreaFunction>().AddParams(_size)
                                                  .ExecuteAndGetResult<Vector2Int?>();

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
