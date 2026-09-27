using System.Collections.Generic;
using FlowIoC.BaseModule.Adapters;
using FlowIoC.BaseModule.Constructables;
using FlowIoC.BaseModule.Injectable.Attributes;
using Modules.BuildingsModule.RootsContexts;
using Modules.BuildingsModule.Shared.Data.UnityObjects;
using Modules.BuildingsModule.Shared.Data.ValueObjects;
using Modules.BuildingsModule.Shared.Enums;
using UnityEngine;

namespace Modules.BuildingsModule.Models
{
    /// <summary>Reads CD_Buildings off the Root's adapter in PostConstruct and hands its entries out.</summary>
    internal class BuildingsModel : IBuildingsModel, IConstructable
    {
        [Inject(nameof(BuildingsSystemContext))]
        private GameObject _root { get; set; }

        public bool IsPostConstructed { get; set; }
        public bool IsDeconstructed { get; set; }

        public IReadOnlyDictionary<BuildType, BuildingCVO> Buildings { get; private set; }
        public Transform BoardParent => _root.transform;

        public void PostConstruct() =>
            Buildings = _root.GetComponent<RootAdapter>().GetScriptable<CD_Buildings>().Buildings;

        public void Deconstruct() { }
    }
}
