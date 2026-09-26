using System;
using FlowIoC.BaseModule.Controller;
using FlowIoC.BaseModule.Injectable.Attributes;
using FlowIoC.ConsoleModule;
using FlowIoC.ScreenModule.Service;
using Modules.GameplayModule.InformationScreenModule.ViewsMediators;

namespace Modules.GameplayModule.InformationScreenModule.Controllers
{
    internal class OpenInformationScreenCommand : Command
    {
        [Inject] private IScreenService _screenService { get; set; }

        /// <summary>
        /// Three ways out, and every one of them resolves the retain: the screen opened, the screen
        /// came back null, and the await threw.
        /// </summary>
        public override async void Execute()
        {
            Retain();

            try
            {
                InformationScreenView screen = await _screenService.Open<InformationScreenView>().Show<InformationScreenView>();

                if (screen == null)
                {
                    FlowLogger.LogError("OpenInformationScreenCommand - the screen did not open.");
                    Stop();
                    return;
                }

                Release();
            }
            catch (Exception exception)
            {
                FlowLogger.LogError($"OpenInformationScreenCommand threw while opening the screen: {exception}");
                Stop();
            }
        }
    }
}
