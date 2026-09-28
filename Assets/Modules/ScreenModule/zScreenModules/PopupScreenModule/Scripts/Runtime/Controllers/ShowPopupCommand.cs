using System;
using FlowIoC.BaseModule.Controller;
using FlowIoC.BaseModule.Injectable.Attributes;
using FlowIoC.ConsoleModule;
using FlowIoC.ScreenModule.Service;
using Modules.ScreenModule.PopupScreenModule.Models;
using Modules.ScreenModule.PopupScreenModule.ViewsMediators;

namespace Modules.ScreenModule.PopupScreenModule.Controllers
{
    /// <summary>
    /// Puts the message on the popup, opening it first when it is closed - opening a screen that is already open is
    /// refused, so a second message while it is open only replaces the first.
    /// </summary>
    internal class ShowPopupCommand : Command
    {
        [Inject]      private IScreenService     _screenService { get; set; }
        [Inject]      private IPopupScreenModel  _popupModel    { get; set; }
        [SignalParam] private string             _message       { get; set; }

        /// <summary>
        /// Three ways out of the await, and every one of them resolves the retain: the screen opened, the screen came
        /// back null, and the await threw.
        /// </summary>
        public override async void Execute()
        {
            Retain();

            if (_screenService.TryGet.Screen(out PopupScreenView openScreen))
            {
                openScreen.ShowMessage(_message);
                Release();
                return;
            }

            try
            {
                // The icon and the animation are read as the screen activates, before it plays - so they go in as a parameter.
                PopupScreenView screen = await _screenService.Open<PopupScreenView>()
                                                             .SetParameters(_popupModel.Style)
                                                             .Show<PopupScreenView>();

                if (screen == null)
                {
                    FlowLogger.LogError("ShowPopupCommand - the popup did not open.");
                    Stop();
                    return;
                }

                screen.ShowMessage(_message);
                Release();
            }
            catch (Exception exception)
            {
                FlowLogger.LogError($"ShowPopupCommand threw while opening the popup: {exception}");
                Stop();
            }
        }
    }
}
