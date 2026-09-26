using Modules.BuildingsModule.Data.ValueObjects;
using UnityEngine;

namespace Modules.BuildingsModule.Models
{
    /// <summary>The building waiting on the board for the player to confirm or cancel it, and how it is being moved.</summary>
    internal interface IPlacementModel
    {
        /// <summary>The placement shown as a preview and waiting for the player; null when none is.</summary>
        BuildingPlacementVO PendingPlacement { get; set; }

        /// <summary>Whether the pending placement is held by a press and follows the pointer.</summary>
        bool IsDragging { get; set; }

        /// <summary>From the cell the pending placement was grabbed at to its bottom-left cell, kept while it is dragged.</summary>
        Vector2Int GrabOffset { get; set; }
    }
}
