using UnityEngine;

namespace Modules.GameBoardModule.Shared.Data.ValueObjects
{
    /// <summary>One cell of the game board. Other modules read cells to place and move things on the grid.</summary>
    public class CellVO
    {
        /// <summary>World position of the cell's centre.</summary>
        public Vector2 Position { get; }

        public CellVO(Vector2 position)
        {
            Position = position;
        }
    }
}
