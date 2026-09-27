using System.Collections.Generic;
using Modules.BuildingsModule.Data.ValueObjects;

namespace Modules.BuildingsModule.Models
{
    /// <summary>The buildings standing on the board and the one the player has selected.</summary>
    internal interface IBoardBuildingsModel
    {
        /// <summary>Every building on the board, by the entity id the grid gave its cells.</summary>
        Dictionary<int, BuildingRecordVO> Buildings { get; }

        /// <summary>The building the player selected on the board; null when none is.</summary>
        BuildingRecordVO Selected { get; set; }
    }
}
