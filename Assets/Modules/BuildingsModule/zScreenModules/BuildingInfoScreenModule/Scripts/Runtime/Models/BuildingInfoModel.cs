using System.Collections.Generic;
using FlowIoC.BaseModule.Adapters;
using FlowIoC.BaseModule.Constructables;
using FlowIoC.BaseModule.Injectable.Attributes;
using FlowIoC.BaseModule.SharedData;
using Modules.BuildingsModule.BuildingInfoScreenModule.RootsContexts;
using Modules.BuildingsModule.Shared.Data.UnityObjects;
using Modules.BuildingsModule.Shared.Data.ValueObjects;
using Modules.BuildingsModule.Shared.Enums;
using Modules.UnitsModule.Shared.Data.UnityObjects;
using Modules.UnitsModule.Shared.Data.ValueObjects;
using Modules.UnitsModule.Shared.Enums;
using UnityEngine;

namespace Modules.BuildingsModule.BuildingInfoScreenModule.Models
{
    /// <summary>
    /// Reads CD_Buildings off the Root's adapter in PostConstruct - the screen's context is listed on
    /// BuildingsSystemRoot, so that is the Root - and CD_Units through ISharedDataModel, where
    /// UnitsSystemRoot files it for every module that shows units.
    /// </summary>
    internal class BuildingInfoModel : IBuildingInfoModel, IConstructable
    {
        [Inject(nameof(BuildingInfoScreenContext))]
        private GameObject _root { get; set; }

        [Inject] private ISharedDataModel _sharedDataModel { get; set; }

        public bool IsPostConstructed { get; set; }
        public bool IsDeconstructed { get; set; }

        public IReadOnlyDictionary<BuildType, BuildingCVO> Buildings { get; private set; }
        public IReadOnlyDictionary<UnitType, UnitCVO> Units { get; private set; }

        public void PostConstruct()
        {
            Buildings = _root.GetComponent<RootAdapter>().GetScriptable<CD_Buildings>().Buildings;
            Units = _sharedDataModel.GetScriptable<CD_Units>().Units;
        }

        public void Deconstruct()
        {
        }
    }
}
