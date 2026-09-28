#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using Modules.BuildingsModule.Shared.Enums;
using Modules.UnitsModule.Shared.Enums;
using UnityEngine;

namespace Modules.MainModule.MainTestModule.Data.UnityObjects
{
    /// <summary>
    /// The board the design test scene starts with: which building and which soldier stands where, in cells
    /// ((0, 0) is the bottom-left cell). Everything starts at full health. Filed in the Shared Scriptables of
    /// MainTestRoot's adapter in MainTestScene; MainTestContext puts it on the board as soon as the board is built.
    /// </summary>
    [CreateAssetMenu(fileName = "RD_Grid_Test", menuName = "Game/Test/RD_Grid_Test")]
    public class RD_GridTest : ScriptableObject
    {
        [Serializable]
        public class BuildingEntry
        {
            public BuildType Type;

            [Tooltip("The building's bottom-left cell; it covers its CD_Buildings Size from there.")]
            public Vector2Int Origin;
        }

        [Serializable]
        public class UnitEntry
        {
            public UnitType Type;
            public Vector2Int Cell;
        }

        public List<BuildingEntry> Buildings = new();
        public List<UnitEntry> Units = new();
    }
}
#endif
