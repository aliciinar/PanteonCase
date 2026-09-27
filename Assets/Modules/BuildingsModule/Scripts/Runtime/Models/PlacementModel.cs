using FlowIoC.BaseModule.Constructables;
using FlowIoC.BaseModule.Injectable.Attributes;
using FlowIoC.BaseModule.SharedData;
using Modules.BuildingsModule.Data.ValueObjects;
using Modules.GameplayModule.Shared.Data.UnityObjects;
using UnityEngine;

namespace Modules.BuildingsModule.Models
{
    /// <summary>The waiting placement, and RD_GameStatus - read through ISharedDataModel, where GameplaySystemRoot files it.</summary>
    internal class PlacementModel : IPlacementModel, IConstructable
    {
        [Inject] private ISharedDataModel _sharedDataModel { get; set; }

        public bool IsPostConstructed { get; set; }
        public bool IsDeconstructed { get; set; }

        public BuildingPlacementVO Pending { get; private set; }
        public bool IsWaiting => Pending != null;
        public bool IsDragging { get; private set; }
        public Vector2Int GrabOffset { get; private set; }
        public bool IsGameLocked => _gameStatus.IsLocked;

        private RD_GameStatus _gameStatus;

        public void PostConstruct() => _gameStatus = _sharedDataModel.GetScriptable<RD_GameStatus>();

        public void Deconstruct()
        {
        }

        public void Wait(BuildingPlacementVO placement) => Pending = placement;

        public void Grab(Vector2Int grabOffset)
        {
            GrabOffset = grabOffset;
            IsDragging = true;
        }

        public void Release() => IsDragging = false;

        public BuildingPlacementVO Take()
        {
            BuildingPlacementVO placement = Pending;
            Discard();
            return placement;
        }

        public void Discard()
        {
            Pending = null;
            IsDragging = false;
        }
    }
}
