using System;
using FlowIoC.BaseModule.Controller;
using FlowIoC.BaseModule.Injectable.Attributes;
using FlowIoC.ConsoleModule;
using FlowIoC.ScreenModule.Service;
using Modules.BuildingsModule.ProductionMenuScreenModule.ViewsMediators;

namespace Modules.BuildingsModule.ProductionMenuScreenModule.Controllers
{
    internal class OpenProductionMenuScreenCommand : Command
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
                ProductionMenuScreenView screen = await _screenService.Open<ProductionMenuScreenView>().Show<ProductionMenuScreenView>();

                if (screen == null)
                {
                    FlowLogger.LogError("OpenProductionMenuScreenCommand - the screen did not open.");
                    Stop();
                    return;
                }

                Release();
            }
            catch (Exception exception)
            {
                FlowLogger.LogError($"OpenProductionMenuScreenCommand threw while opening the screen: {exception}");
                Stop();
            }
        }
    }
}
