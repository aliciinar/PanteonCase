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

        /// <summary>The colour a selected unit wears.</summary>
        Color SelectedColor { get; }

        /// <summary>How a struck unit flashes.</summary>
        HitFlashCVO HitFlash { get; }

        /// <summary>The puff a destroyed unit leaves.</summary>
        UnitExplosionCVO Explosion { get; }

        /// <summary>What the player is told when the selected unit has no way to where it was ordered, or to any side of what it was ordered to attack.</summary>
        string NoWayMessage { get; }

        /// <summary>What the player is told when a requested unit can walk from its building's door to no free cell.</summary>
        string NoRoomToSpawnMessage { get; }

        /// <summary>Whether an action is running (RD_GameStatus): then no unit is made.</summary>
        bool IsGameLocked { get; }

        /// <summary>Where the unit objects on the board hang: the module's Root.</summary>
        Transform BoardParent { get; }
    }
}
