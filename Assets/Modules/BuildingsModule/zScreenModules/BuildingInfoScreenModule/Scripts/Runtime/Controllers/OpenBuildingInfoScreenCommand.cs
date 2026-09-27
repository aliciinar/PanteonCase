using System;
using FlowIoC.BaseModule.Controller;
using FlowIoC.BaseModule.Injectable.Attributes;
using FlowIoC.ConsoleModule;
using FlowIoC.ScreenModule.Service;
using Modules.BuildingsModule.BuildingInfoScreenModule.ViewsMediators;

namespace Modules.BuildingsModule.BuildingInfoScreenModule.Controllers
{
    /// <summary>
    /// Hands the screen to the next step, opening it first when it is closed - opening a screen that
    /// is already open is refused, and a new selection while it is open only refills it.
    /// </summary>
    internal class OpenBuildingInfoScreenCommand : Command
    {
        [Inject] private IScreenService _screenService { get; set; }

        /// <summary>
        /// Three ways out of the await, and every one of them resolves the retain: the screen opened,
        /// the screen came back null, and the await threw.
        /// </summary>
        public override async void Execute()
        {
            Retain();

            if (_screenService.TryGet.Screen(out BuildingInfoScreenView openScreen))
            {
                Release(openScreen);
                return;
            }

            try
            {
                BuildingInfoScreenView screen = await _screenService.Open<BuildingInfoScreenView>().Show<BuildingInfoScreenView>();

                if (screen == null)
                {
                    FlowLogger.LogError("OpenBuildingInfoScreenCommand - the screen did not open.");
                    Stop();
                    return;
                }

                Release(screen);
            }
            catch (Exception exception)
            {
                FlowLogger.LogError($"OpenBuildingInfoScreenCommand threw while opening the screen: {exception}");
                Stop();
            }
        }
    }
}
