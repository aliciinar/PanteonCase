using FlowIoC.BaseModule.Injectable.Components;
using FlowIoC.BaseModule.ViewsMediators.View;
using Modules.BuildingsModule.Entities;
using Modules.BuildingsModule.Shared.Enums;
using UnityEngine;

namespace Modules.BuildingsModule.ViewsMediators
{
    /// <summary>The buildings standing on the board, parented under this object and named after what they are.</summary>
    [RequireComponent(typeof(ViewInjector))]
    public class PlacedBuildingsView : MonoBehaviour, IView
    {
        public bool IsRegistered { get; set; }

        /// <param name="building">A pooled building, taken out of the pool for this.</param>
        /// <param name="type">Which building it is; names it in the Hierarchy.</param>
        /// <param name="sprite">What the building looks like.</param>
        /// <param name="area">World rect the building covers - its footprint on the board.</param>
        public void PlaceBuilding(BoardBuilding building, BuildType type, Sprite sprite, Rect area)
        {
            building.name = type.ToString();
            building.transform.SetParent(transform, false);
            building.Show(sprite, area);
        }
    }
}
