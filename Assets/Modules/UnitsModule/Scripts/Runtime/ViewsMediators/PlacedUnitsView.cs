using FlowIoC.BaseModule.Injectable.Components;
using FlowIoC.BaseModule.ViewsMediators.View;
using Modules.UnitsModule.Entities;
using Modules.UnitsModule.Shared.Enums;
using UnityEngine;

namespace Modules.UnitsModule.ViewsMediators
{
    /// <summary>The units on the board, parented under this object and named after what they are.</summary>
    [RequireComponent(typeof(ViewInjector))]
    public class PlacedUnitsView : MonoBehaviour, IView
    {
        public bool IsRegistered { get; set; }

        /// <param name="unit">A pooled unit, taken out of the pool for this.</param>
        /// <param name="type">Which unit it is; names it in the Hierarchy.</param>
        /// <param name="sprite">What the unit looks like.</param>
        /// <param name="spawnArea">World rect of the cell it appears on.</param>
        /// <param name="waypoints">World points it walks through; empty when it stays where it appeared.</param>
        /// <param name="cellsPerSecond">Walking speed.</param>
        public void PlaceUnit(BoardUnit unit, UnitType type, Sprite sprite, Rect spawnArea, Vector3[] waypoints, float cellsPerSecond)
        {
            unit.name = type.ToString();
            unit.transform.SetParent(transform, false);
            unit.Show(sprite, spawnArea);

            if (waypoints.Length > 0) unit.MoveAlong(waypoints, cellsPerSecond);
        }
    }
}
