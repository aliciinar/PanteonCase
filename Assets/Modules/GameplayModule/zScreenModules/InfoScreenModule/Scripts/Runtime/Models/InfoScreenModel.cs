using System.Collections.Generic;
using FlowIoC.BaseModule.Constructables;
using FlowIoC.BaseModule.Injectable.Attributes;
using FlowIoC.BaseModule.SharedData;
using Modules.BuildingsModule.Shared.Data.UnityObjects;
using Modules.BuildingsModule.Shared.Data.ValueObjects;
using Modules.BuildingsModule.Shared.Enums;
using Modules.GameplayModule.InfoScreenModule.Enums;
using Modules.UnitsModule.Shared.Data.UnityObjects;
using Modules.UnitsModule.Shared.Data.ValueObjects;
using Modules.UnitsModule.Shared.Enums;

namespace Modules.GameplayModule.InfoScreenModule.Models
{
    /// <summary>
    /// Reads CD_Buildings and CD_Units through ISharedDataModel in PostConstruct - BuildingsSystemRoot and
    /// UnitsSystemRoot file them in their adapters' Shared Scriptables for every module that shows buildings or
    /// units - and keeps which of the two the screen shows.
    /// </summary>
    internal class InfoScreenModel : IInfoScreenModel, IConstructable
    {
        [Inject] private ISharedDataModel _sharedDataModel { get; set; }

        public bool IsPostConstructed { get; set; }
        public bool IsDeconstructed { get; set; }

        public IReadOnlyDictionary<BuildType, BuildingCVO> Buildings { get; private set; }
        public IReadOnlyDictionary<UnitType, UnitCVO> Units { get; private set; }
        public InfoSubjectType Shown { get; private set; }

        public void PostConstruct()
        {
            Buildings = _sharedDataModel.GetScriptable<CD_Buildings>().Buildings;
            Units = _sharedDataModel.GetScriptable<CD_Units>().Units;
        }

        public void Deconstruct() => Shown = InfoSubjectType.None;

        public void SetShown(InfoSubjectType subject) => Shown = subject;
    }
}
