using FlowIoC.BaseModule.Controller;
using FlowIoC.BaseModule.Injectable.Attributes;
using FlowIoC.ScreenModule.Service;
using Modules.GameplayModule.InformationScreenModule.Signals;
using Modules.GameplayModule.InformationScreenModule.ViewsMediators;

namespace Modules.GameplayModule.InformationScreenModule.Controllers
{
    /// <summary>
    /// Announces the screen area the information panel covers - after it opened, or after the window
    /// was resized - so the camera can draw next to it.
    /// </summary>
    internal class ReportInformationAreaCommand : Command
    {
        [Inject]       private IScreenService           _screenService { get; set; }
        [InjectSignal] private InformationScreenSignals _signals       { get; set; }

        public override void Execute()
        {
            // Not open (a resize while the game is still loading): there is nothing to measure.
            if (!_screenService.TryGet.Screen(out InformationScreenView screen))
            {
                Stop();
                return;
            }

            _signals.Outgoing.AreaChanged.Dispatch(screen.MeasureArea());
        }
    }
}
