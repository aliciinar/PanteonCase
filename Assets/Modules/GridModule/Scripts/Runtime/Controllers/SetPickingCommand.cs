using FlowIoC.BaseModule.Controller;
using FlowIoC.BaseModule.Injectable.Attributes;
using Modules.GridModule.Models;

namespace Modules.GridModule.Controllers
{
    /// <summary>Whether presses on the board are picked from now on - fixed where the step is bound.</summary>
    internal class SetPickingCommand : Command<bool>
    {
        [Inject] private IGridModel _gridModel { get; set; }

        public override void Execute(bool isPicking) => _gridModel.IsPicking = isPicking;
    }
}
