using System.Collections.Generic;
using Modules.BuildingsModule.Shared.Data.ValueObjects;
using Modules.BuildingsModule.Shared.Enums;

namespace Modules.BuildingsModule.Models
{
    /// <summary>Every building as CD_Buildings authors it.</summary>
    internal interface IBuildingsModel
    {
        IReadOnlyDictionary<BuildType, BuildingCVO> Buildings { get; }
    }
}
