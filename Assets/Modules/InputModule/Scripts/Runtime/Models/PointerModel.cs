using FlowIoC.BaseModule.Constructables;
using FlowIoC.BaseModule.Injectable.Attributes;
using FlowIoC.BaseModule.SharedData;
using Modules.GameplayModule.Shared.Data.UnityObjects;

namespace Modules.InputModule.Models
{
    /// <summary>The current press, and RD_GameStatus - read through ISharedDataModel, where GameplaySystemRoot files it.</summary>
    internal class PointerModel : IPointerModel, IConstructable
    {
        [Inject] private ISharedDataModel _sharedDataModel { get; set; }

        public bool IsPostConstructed { get; set; }
        public bool IsDeconstructed { get; set; }

        public bool IsPressOnWorld { get; private set; }
        public bool IsGameLocked => _gameStatus.IsLocked;

        private RD_GameStatus _gameStatus;

        public void PostConstruct() => _gameStatus = _sharedDataModel.GetScriptable<RD_GameStatus>();

        public void Deconstruct()
        {
        }

        public void BeginPress(bool onWorld) => IsPressOnWorld = onWorld;

        public void EndPress() => IsPressOnWorld = false;
    }
}
