using FlowIoC.BaseModule.Injectable.Attributes;
using FlowIoC.BaseModule.ViewsMediators.Mediator;

namespace Modules.GameplayModule.InformationScreenModule.ViewsMediators
{
    /// <summary>
    /// The panel has no input yet. What it shows and the area it reports are handled by the commands,
    /// which reach the open screen through the screen service.
    /// </summary>
    public class InformationScreenMediator : IMediator
    {
        [Inject] private InformationScreenView _view { get; set; }

        public void OnRegister()
        {
        }

        public void OnRemove()
        {
        }
    }
}
