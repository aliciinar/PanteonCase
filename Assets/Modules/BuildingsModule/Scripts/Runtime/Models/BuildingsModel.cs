using System.Collections.Generic;
using FlowIoC.BaseModule.Constructables;
using FlowIoC.BaseModule.Injectable.Attributes;
using FlowIoC.BaseModule.SharedData;
using Modules.BuildingsModule.RootsContexts;
using Modules.BuildingsModule.Shared.Data.UnityObjects;
using Modules.BuildingsModule.Shared.Data.ValueObjects;
using Modules.BuildingsModule.Shared.Enums;
using UnityEngine;

namespace Modules.BuildingsModule.Models
{
    /// <summary>
    /// Reads CD_Buildings in PostConstruct and hands its entries out. The asset is filed in the Shared Scriptables of
    /// BuildingsSystemRoot's adapter - the information panel shows buildings too - so it is read through
    /// ISharedDataModel like any reader would. The building objects on the board hang under the module's Root.
    /// </summary>
    internal class BuildingsModel : IBuildingsModel, IConstructable
    {
        [Inject(nameof(BuildingsSystemContext))]
        private GameObject _root { get; set; }

        [Inject] private ISharedDataModel _sharedDataModel { get; set; }

        public bool IsPostConstructed { get; set; }
        public bool IsDeconstructed { get; set; }

        public IReadOnlyDictionary<BuildType, BuildingCVO> Buildings { get; private set; }
        public Color SelectedTint { get; private set; }
        public Transform BoardParent => _root.transform;

        public void PostConstruct()
        {
            var config = _sharedDataModel.GetScriptable<CD_Buildings>();
            Buildings = config.Buildings;
            SelectedTint = config.SelectedTint;
        }

        public void Deconstruct() { }
    }
}
