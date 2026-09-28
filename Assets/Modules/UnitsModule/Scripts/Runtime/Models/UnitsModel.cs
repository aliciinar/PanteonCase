using System.Collections.Generic;
using FlowIoC.BaseModule.Constructables;
using FlowIoC.BaseModule.Injectable.Attributes;
using FlowIoC.BaseModule.SharedData;
using Modules.GameplayModule.Shared.Data.UnityObjects;
using Modules.UnitsModule.Data.ValueObjects;
using Modules.UnitsModule.RootsContexts;
using Modules.UnitsModule.Shared.Data.UnityObjects;
using Modules.UnitsModule.Shared.Data.ValueObjects;
using Modules.UnitsModule.Shared.Enums;
using UnityEngine;

namespace Modules.UnitsModule.Models
{
    /// <summary>
    /// Reads CD_Units in PostConstruct and hands its entries out. The asset is filed in the Shared
    /// Scriptables of UnitsSystemRoot's adapter - other modules show units too - so it is read through
    /// ISharedDataModel like any reader would. The unit objects on the board hang under the module's Root.
    /// </summary>
    internal class UnitsModel : IUnitsModel, IConstructable
    {
        [Inject(nameof(UnitsSystemContext))]
        private GameObject _root { get; set; }

        [Inject] private ISharedDataModel _sharedDataModel { get; set; }

        public bool IsPostConstructed { get; set; }
        public bool IsDeconstructed { get; set; }

        public IReadOnlyDictionary<UnitType, UnitCVO> Units { get; private set; }
        public Color SelectedTint { get; private set; }
        public Color HitFlash { get; private set; }
        public float HitFlashDuration { get; private set; }
        public UnitExplosionVO Explosion { get; private set; }
        public string NoRoomToAttackMessage { get; private set; }
        public string NoWayMessage { get; private set; }
        public bool IsGameLocked => _gameStatus.IsLocked;
        public Transform BoardParent => _root.transform;

        private RD_GameStatus _gameStatus;

        public void PostConstruct()
        {
            var config = _sharedDataModel.GetScriptable<CD_Units>();
            Units = config.Units;
            SelectedTint = config.SelectedTint;
            HitFlash = config.HitFlash;
            HitFlashDuration = config.HitFlashDuration;
            Explosion = new UnitExplosionVO(config.ExplosionSprites, config.ExplosionSize, config.ExplosionDuration);
            NoRoomToAttackMessage = config.NoRoomToAttackMessage;
            NoWayMessage = config.NoWayMessage;

            // Filed by GameplaySystemRoot for every module that takes an order.
            _gameStatus = _sharedDataModel.GetScriptable<RD_GameStatus>();
        }

        public void Deconstruct()
        {
        }
    }
}
