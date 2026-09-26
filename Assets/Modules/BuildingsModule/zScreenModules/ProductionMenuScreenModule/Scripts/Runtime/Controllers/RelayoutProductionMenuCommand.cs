using FlowIoC.BaseModule.Controller;
using FlowIoC.BaseModule.Injectable.Attributes;
using FlowIoC.ScreenModule.Service;
using Modules.BuildingsModule.ProductionMenuScreenModule.Signals;
using Modules.BuildingsModule.ProductionMenuScreenModule.ViewsMediators;

namespace Modules.BuildingsModule.ProductionMenuScreenModule.Controllers
{
    /// <summary>
    /// Lays the menu out against the screen as it is now - after it opened, or after the window was
    /// resized: puts the rows back in their columns, announces the area the panel covers (the camera
    /// draws next to it), and has the visible rows worked out again.
    /// </summary>
    internal class RelayoutProductionMenuCommand : Command
    {
        [Inject]       private IScreenService                      _screenService   { get; set; }
        [InjectSignal] private ProductionMenuScreenSignals         _signals         { get; set; }
        [InjectSignal] private ProductionMenuScreenInternalSignals _internalSignals { get; set; }

        public override void Execute()
        {
            Retain();

            // Not open (a resize while the game is still loading): there is nothing to lay out.
            if (!_screenService.TryGet.Screen(out ProductionMenuScreenView screen))
            {
                Stop();
                return;
            }

            screen.RepositionRows();
            _signals.Outgoing.AreaChanged.Dispatch(screen.MeasureArea());
            _internalSignals.Scrolled.Dispatch();

            Release();
        }
    }
}
