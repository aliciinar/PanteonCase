using FlowIoC.BaseModule.Controller;
using FlowIoC.BaseModule.Injectable.Attributes;
using FlowIoC.ScreenModule.Service;
using Modules.GameplayModule.InfoScreenModule.Enums;
using Modules.GameplayModule.InfoScreenModule.Models;
using Modules.GameplayModule.InfoScreenModule.ViewsMediators;

namespace Modules.GameplayModule.InfoScreenModule.Controllers
{
    /// <summary>
    /// A selection of the bound kind was cleared: the screen closes when it is open and still shows that kind - a
    /// unit selected while a building's selection is cleared, in whichever order the two arrive, stays on screen.
    /// Its cards go back to the pool once it has closed.
    /// </summary>
    internal class HideInfoScreenCommand : Command<InfoSubjectType>
    {
        [Inject] private IInfoScreenModel _infoScreenModel { get; set; }
        [Inject] private IScreenService   _screenService   { get; set; }

        public override void Execute(InfoSubjectType cleared)
        {
            if (_infoScreenModel.Shown != cleared) return;

            _infoScreenModel.SetShown(InfoSubjectType.None);

            if (_screenService.TryGet.Screen(out InfoScreenView screen))
                _screenService.Hide.Screen(screen);
        }
    }
}
