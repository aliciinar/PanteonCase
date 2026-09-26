using UnityEngine;

namespace Modules.GameBoardModule.Shared.Data.ValueObjects
{
    /// <summary>
    /// One cell of the game board: where it is and what stands on it. Other modules read cells to place
    /// and move things on the grid; only GameBoardModule writes what occupies them.
    /// </summary>
    public class CellVO
    {
        /// <summary>World position of the cell's centre.</summary>
        public Vector2 Position { get; }

        /// <summary>What stands on the cell, or null when nothing does. Written by GameBoardModule only.</summary>
        public CellOccupantVO Occupant { get; internal set; }

        public bool IsFree => Occupant == null;

        public CellVO(Vector2 position)
        {
            Position = position;
        }
    }
}
