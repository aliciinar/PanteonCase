using System.Collections.Generic;
using FlowIoC.BaseModule.Constructables;
using FlowIoC.BaseModule.Injectable.Attributes;
using FlowIoC.BaseModule.SharedData;
using Modules.UnitsModule.Shared.Data.UnityObjects;
using Modules.UnitsModule.Shared.Data.ValueObjects;
using Modules.UnitsModule.Shared.Enums;

namespace Modules.UnitsModule.Models
{
    /// <summary>
    /// Reads CD_Units in PostConstruct and hands its entries out. The asset is filed in the Shared
    /// Scriptables of UnitsSystemRoot's adapter - other modules show units too - so it is read through
    /// ISharedDataModel like any reader would.
    /// </summary>
    internal class UnitsModel : IUnitsModel, IConstructable
    {
        [Inject] private ISharedDataModel _sharedDataModel { get; set; }

        public bool IsPostConstructed { get; set; }
        public bool IsDeconstructed { get; set; }

        public IReadOnlyDictionary<UnitType, UnitCVO> Units { get; private set; }

        public void PostConstruct() => Units = _sharedDataModel.GetScriptable<CD_Units>().Units;

        public void Deconstruct()
        {
        }
    }
}
