using System.Collections.Generic;
using Modules.UnitsModule.Shared.Data.ValueObjects;
using Modules.UnitsModule.Shared.Enums;
using UnityEngine;

namespace Modules.UnitsModule.Models
{
    /// <summary>Every unit as CD_Units authors it.</summary>
    internal interface IUnitsModel
    {
        IReadOnlyDictionary<UnitType, UnitCVO> Units { get; }

        /// <summary>The colour a selected unit is tinted with.</summary>
        Color SelectedTint { get; }
    }
}
