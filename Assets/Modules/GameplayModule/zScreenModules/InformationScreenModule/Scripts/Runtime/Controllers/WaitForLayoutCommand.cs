using FlowIoC.BaseModule.Controller;
using FlowIoC.BaseModule.Injectable.Attributes;
using FlowIoC.BaseModule.Provider.Coroutine;

namespace Modules.GameplayModule.InformationScreenModule.Controllers
{
    /// <summary>
    /// Holds the sequence until the UI has laid itself out for a new window size. A resize is reported
    /// during one frame, but the screen canvas's CanvasScaler only rescales in the next frame's Update,
    /// so anything measured before the end of that frame would still be the old layout.
    /// </summary>
    internal class WaitForLayoutCommand : Command
    {
        private const int FramesToWait = 2;

        [Inject] private ICoroutineProvider _coroutineProvider { get; set; }

        public override void Execute()
        {
            Retain();
            _coroutineProvider.WaitForEndOfFrames(FramesToWait, () => Release());
        }
    }
}
