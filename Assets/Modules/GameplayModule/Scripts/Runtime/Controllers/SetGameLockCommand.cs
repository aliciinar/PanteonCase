using FlowIoC.BaseModule.Controller;
using FlowIoC.BaseModule.Injectable.Attributes;
using Modules.GameplayModule.Models;

namespace Modules.GameplayModule.Controllers
{
    /// <summary>Locks the game while an action runs, or unlocks it when the action ends - fixed where the step is bound.</summary>
    internal class SetGameLockCommand : Command<bool>
    {
        [Inject] private IGameStatusModel _gameStatusModel { get; set; }

        public override void Execute(bool isLocked) => _gameStatusModel.SetLocked(isLocked);
    }
}
