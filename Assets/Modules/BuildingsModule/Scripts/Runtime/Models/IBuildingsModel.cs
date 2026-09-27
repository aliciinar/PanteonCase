using System.Collections.Generic;
using Modules.BuildingsModule.Shared.Data.ValueObjects;
using Modules.BuildingsModule.Shared.Enums;
using UnityEngine;

namespace Modules.BuildingsModule.Models
{
    /// <summary>Every building as CD_Buildings authors it.</summary>
    internal interface IBuildingsModel
    {
        IReadOnlyDictionary<BuildType, BuildingCVO> Buildings { get; }

        /// <summary>The colour a selected building wears on the board.</summary>
        Color SelectedTint { get; }

        /// <summary>Where the building objects on the board hang: the module's Root.</summary>
        Transform BoardParent { get; }
    }
}
