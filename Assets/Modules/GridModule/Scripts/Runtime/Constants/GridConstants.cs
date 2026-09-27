using UnityEngine;

namespace Modules.GridModule.Constants
{
    internal static class GridConstants
    {
        /// <summary>The four cells a search steps to from a cell, in the order it tries them. A diagonal is never a step.</summary>
        public static readonly Vector2Int[] NeighbourSteps =
        {
            Vector2Int.up, Vector2Int.right, Vector2Int.down, Vector2Int.left
        };
    }
}
