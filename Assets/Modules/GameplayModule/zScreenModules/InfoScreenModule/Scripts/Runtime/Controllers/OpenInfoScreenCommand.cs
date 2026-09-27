using System;
using FlowIoC.BaseModule.Controller;
using FlowIoC.BaseModule.Injectable.Attributes;
using FlowIoC.ConsoleModule;
using FlowIoC.ScreenModule.Service;
using Modules.GameplayModule.InfoScreenModule.ViewsMediators;

namespace Modules.GameplayModule.InfoScreenModule.Controllers
{
    /// <summary>
    /// Hands the screen to the next step, opening it first when it is closed - opening a screen that
    /// is already open is refused, and a new selection while it is open only refills it - a building or a unit alike.
    /// </summary>
    internal class OpenInfoScreenCommand : Command
    {
        [Inject] private IScreenService _screenService { get; set; }

        /// <summary>
        /// Three ways out of the await, and every one of them resolves the retain: the screen opened,
        /// the screen came back null, and the await threw.
        /// </summary>
        public override async void Execute()
        {
            Retain();

            if (_screenService.TryGet.Screen(out InfoScreenView openScreen))
            {
                Release(openScreen);
                return;
            }

            try
            {
                InfoScreenView screen = await _screenService.Open<InfoScreenView>().Show<InfoScreenView>();

                if (screen == null)
                {
                    FlowLogger.LogError("OpenInfoScreenCommand - the screen did not open.");
                    Stop();
                    return;
                }

                Release(screen);
            }
            catch (Exception exception)
            {
                FlowLogger.LogError($"OpenInfoScreenCommand threw while opening the screen: {exception}");
                Stop();
            }
        }
    }
}
