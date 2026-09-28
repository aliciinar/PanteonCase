using System.Collections.Generic;
using Modules.BuildingsModule.Data.ValueObjects;
using Modules.BuildingsModule.Shared.Data.ValueObjects;
using Modules.BuildingsModule.Shared.Enums;
using UnityEngine;

namespace Modules.BuildingsModule.Models
{
    /// <summary>Every building as CD_Buildings authors it, and how buildings look on the board.</summary>
    internal interface IBuildingsModel
    {
        IReadOnlyDictionary<BuildType, BuildingCVO> Buildings { get; }

        /// <summary>The colour a selected building wears on the board.</summary>
        Color SelectedTint { get; }

        /// <summary>The colour a struck building flashes with, and how long the flash takes there and back.</summary>
        Color HitFlash { get; }
        float HitFlashDuration { get; }

        /// <summary>How a destroyed building explodes.</summary>
        BuildingExplosionVO Explosion { get; }

        /// <summary>The share of the footprint's width the health bar spans, and how far below its top edge it sits, in cells.</summary>
        float HealthBarWidth { get; }
        float HealthBarInset { get; }

        /// <summary>The placement ghost's colour where the building fits, and where it does not.</summary>
        Color PlacementFitsTint { get; }
        Color PlacementBlockedTint { get; }

        /// <summary>Where the building objects on the board hang: the module's Root.</summary>
        Transform BoardParent { get; }
    }
}
