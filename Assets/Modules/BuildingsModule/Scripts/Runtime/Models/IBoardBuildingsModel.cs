using Modules.BuildingsModule.Data.ValueObjects;

namespace Modules.BuildingsModule.Models
{
    /// <summary>The buildings standing on the board, by the entity id the grid gave their cells, and the one the player has selected.</summary>
    internal interface IBoardBuildingsModel
    {
        /// <summary>The building the player selected on the board; null when none is.</summary>
        BuildingRecordVO Selected { get; }

        /// <summary>A building now stands on the board.</summary>
        void Add(BuildingRecordVO building);

        /// <summary>The building whose cells hold this entity id.</summary>
        BuildingRecordVO Get(int entityId);

        void Select(BuildingRecordVO building);

        /// <summary>Nothing is selected any more. False when nothing was.</summary>
        bool ClearSelection();
    }
}
