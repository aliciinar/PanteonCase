using FlowIoC.BaseModule.Controller;
using FlowIoC.BaseModule.Injectable.Attributes;
using FlowIoC.ScreenModule.Service;
using Modules.BuildingsModule.BuildingInfoScreenModule.ViewsMediators;

namespace Modules.BuildingsModule.BuildingInfoScreenModule.Controllers
{
    /// <summary>Closes the screen when it is open; its cards go back to the pool once it has closed.</summary>
    internal class HideBuildingInfoScreenCommand : Command
    {
        [Inject] private IScreenService _screenService { get; set; }

        public override void Execute()
        {
            if (_screenService.TryGet.Screen(out BuildingInfoScreenView screen))
                _screenService.Hide.Screen(screen);
        }
    }
}
