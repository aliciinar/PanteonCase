using FlowIoC.BaseModule.Constructables;
using FlowIoC.BaseModule.Injectable.Attributes;
using FlowIoC.BaseModule.SharedData;
using Modules.GameplayModule.Shared.Data.UnityObjects;

namespace Modules.GameplayModule.Models
{
    /// <summary>
    /// Keeps RD_GameStatus - filed in the Shared Scriptables of GameplaySystemRoot's adapter, since every module that
    /// takes an order reads it - rather than a copy of it. The asset keeps its values between play sessions in the
    /// Editor, so a session starts and ends unlocked.
    /// </summary>
    internal class GameStatusModel : IGameStatusModel, IConstructable
    {
        [Inject] private ISharedDataModel _sharedDataModel { get; set; }

        public bool IsPostConstructed { get; set; }
        public bool IsDeconstructed { get; set; }

        public bool IsLocked => _status.IsLocked;

        private RD_GameStatus _status;

        public void PostConstruct()
        {
            _status = _sharedDataModel.GetScriptable<RD_GameStatus>();
            _status.IsLocked = false;
        }

        public void Deconstruct() => _status.IsLocked = false;

        public void SetLocked(bool isLocked) => _status.IsLocked = isLocked;
    }
}
