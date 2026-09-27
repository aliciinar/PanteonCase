using System.Collections.Generic;
using Modules.BuildingsModule.Shared.Data.ValueObjects;
using Modules.BuildingsModule.Shared.Enums;
using Modules.UnitsModule.Shared.Data.ValueObjects;
using Modules.UnitsModule.Shared.Enums;

namespace Modules.BuildingsModule.BuildingInfoScreenModule.Models
{
    /// <summary>Every building and every unit as their configs author them - what the screen shows of them.</summary>
    internal interface IBuildingInfoModel
    {
        IReadOnlyDictionary<BuildType, BuildingCVO> Buildings { get; }

        IReadOnlyDictionary<UnitType, UnitCVO> Units { get; }
    }
}
