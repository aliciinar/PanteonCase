using System;
using System.Collections.Generic;
using FlowIoC.BaseModule.Injectable.Components;
using FlowIoC.BaseModule.ViewsMediators.View;
using Modules.GridModule.Shared.Data.ValueObjects;
using Modules.UnitsModule.Entities;
using UnityEngine;

namespace Modules.UnitsModule.ViewsMediators
{
    /// <summary>
    /// The units on the board, parented under this object and named after what they are. It knows which object shows
    /// which unit - the unit as the grid holds it - so a later order about a unit reaches the object that shows it.
    /// </summary>
    [RequireComponent(typeof(ViewInjector))]
    public class PlacedUnitsView : MonoBehaviour, IView
    {
        public bool IsRegistered { get; set; }

        /// <summary>A walking unit starts stepping into the cell whose centre is this world point.</summary>
        public event Action<BoardUnitVO, Vector3> UnitStepped;

        private readonly Dictionary<BoardUnitVO, BoardUnit> _shown = new();

        /// <param name="unit">The unit as the grid holds it.</param>
        /// <param name="view">A pooled unit object, taken out of the pool for this.</param>
        /// <param name="sprite">What the unit looks like.</param>
        /// <param name="spawnArea">World rect of the cell it appears on.</param>
        /// <param name="waypoints">World points it walks through; empty when it stays where it appeared.</param>
        /// <param name="unitsPerSecond">Walking speed in world units.</param>
        public void PlaceUnit(BoardUnitVO unit, BoardUnit view, Sprite sprite, Rect spawnArea, Vector3[] waypoints, float unitsPerSecond)
        {
            view.name = unit.Type.ToString();
            view.transform.SetParent(transform, false);
            view.StepStarted = point => UnitStepped?.Invoke(unit, point);
            view.Show(sprite, spawnArea);
            view.MoveAlong(waypoints, unitsPerSecond);

            _shown.Add(unit, view);
        }

        public void MoveUnit(BoardUnitVO unit, Vector3[] waypoints, float unitsPerSecond) =>
            _shown[unit].MoveAlong(waypoints, unitsPerSecond);

        public void ShowSelected(BoardUnitVO unit, Color tint) => _shown[unit].ShowSelected(tint);

        public void ShowDeselected(BoardUnitVO unit) => _shown[unit].ShowDeselected();
    }
}
