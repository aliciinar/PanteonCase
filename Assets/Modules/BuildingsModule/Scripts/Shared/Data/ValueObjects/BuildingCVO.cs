using System;
using System.Collections.Generic;
using Modules.UnitsModule.Shared.Enums;
using UnityEngine;

namespace Modules.BuildingsModule.Shared.Data.ValueObjects
{
    /// <summary>
    /// One building as the designer authors it: its name, how the menu and the board show it, its footprint,
    /// its health, and - for a building that produces units - what it produces and where they appear.
    /// </summary>
    [Serializable]
    public class BuildingCVO
    {
        [Tooltip("The name the player reads: on the production menu card, under the placement prompt and in the information panel.")]
        public string Name;

        [Tooltip("The building's card image in the production menu.")]
        public Sprite Icon;

        [Tooltip("Drawn over the building's footprint on the board, fitted to it whatever the sprite's own size.")]
        public Sprite BoardSprite;

        [Tooltip("Footprint in cells: columns (x) and rows (y).")]
        [Min(1)] public Vector2Int Size = Vector2Int.one;

        [Tooltip("Health points; the building is destroyed when they reach 0.")]
        [Min(1)] public int Hp = 1;

        [Tooltip("The units this building produces, in the order the information panel lists them. Empty for a building that produces nothing.")]
        public List<UnitType> ProducibleUnits = new();

        [Tooltip("The cell its units walk to once they come out, relative to the building's bottom-left cell, outside " +
                 "the footprint: (2, -3) is three cells below the bottom edge. When it is taken they go to the free cell " +
                 "nearest it. Used only when the building produces units.")]
        public Vector2Int SpawnPoint;

        [Tooltip("The cell of the building its units come out of - its door - relative to the building's bottom-left " +
                 "cell, on the building's edge: (2, 0) is the bottom row, third column. Used only when the building " +
                 "produces units.")]
        public Vector2Int ExitPoint;
    }
}
