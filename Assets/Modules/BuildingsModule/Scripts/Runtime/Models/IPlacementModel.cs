using Modules.BuildingsModule.Data.ValueObjects;
using UnityEngine;

namespace Modules.BuildingsModule.Models
{
    /// <summary>
    /// The building waiting on the board for the player to confirm or cancel it, and whether a press holds it.
    /// It changes only through the steps below, so a placement is never dragged while none waits.
    /// </summary>
    internal interface IPlacementModel
    {
        /// <summary>The placement shown as a preview and waiting for the player; null when none is.</summary>
        BuildingPlacementVO Pending { get; }

        bool IsWaiting { get; }

        /// <summary>Whether the pending placement is held by a press and follows the pointer.</summary>
        bool IsDragging { get; }

        /// <summary>From the cell the pending placement was grabbed at to its bottom-left cell, kept while it is dragged.</summary>
        Vector2Int GrabOffset { get; }

        /// <summary>This placement is the one waiting now - a new pick, or the waiting one moved.</summary>
        void Wait(BuildingPlacementVO placement);

        /// <summary>A press holds the waiting placement at this offset; it follows the pointer until released.</summary>
        void Grab(Vector2Int grabOffset);

        /// <summary>The press ended: the placement stays where it is and stops following the pointer.</summary>
        void Release();

        /// <summary>Hands out the waiting placement - the player confirmed it - and nothing waits any more.</summary>
        BuildingPlacementVO Take();

        /// <summary>Drops the waiting placement - the player cancelled it.</summary>
        void Discard();
    }
}
