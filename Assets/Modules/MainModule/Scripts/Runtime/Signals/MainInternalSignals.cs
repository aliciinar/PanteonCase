using FlowIoC.BaseModule.Signals;
using UnityEngine;

namespace Modules.MainModule.Signals
{
    /// <summary>
    /// What MainModule says to its own commands. Launch is dispatched by MainContext.Launch and
    /// ScreenSizeChanged by ScreenResizeModel; both are handled by MainContext, so the holder
    /// crosses no boundary and has no Incoming or Outgoing - those two halves describe one, and an
    /// internal signal has none to describe.
    /// </summary>
    internal class MainInternalSignals : ISignalHolder
    {
        public Signal Launch = new Signal();

        /// <summary>The window reported a size. Fires on every resize step, so it stays out of the command log.</summary>
        public Signal<Vector2Int> ScreenSizeChanged = new(hideCommandLog: true);
    }
}
