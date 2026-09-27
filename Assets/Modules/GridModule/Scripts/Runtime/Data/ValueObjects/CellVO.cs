using UnityEngine;

namespace Modules.GridModule.Shared.Data.ValueObjects
{
    /// <summary>
    /// One cell of the grid: where it is and what stands on it. Every module may read cells; only the
    /// grid service writes what occupies them.
    /// </summary>
    public class CellVO
    {
        /// <summary>World position of the cell's centre.</summary>
        public Vector2 Position { get; }

        /// <summary>What stands on the cell, or null when nothing does. Written by the grid service only.</summary>
        public CellOccupantVO Occupant { get; internal set; }

        public bool IsFree => Occupant == null;

        public CellVO(Vector2 position)
        {
            Position = position;
        }
    }
}
