using FlowIoC.BaseModule.Signals;
using Modules.BuildingsModule.Shared.Data.ValueObjects;
using Modules.BuildingsModule.Shared.Enums;
using Modules.UnitsModule.Shared.Data.ValueObjects;
using Modules.UnitsModule.Shared.Enums;
using UnityEngine;

namespace Modules.BuildingsModule.Signals
{
    public class BuildingsSignals : ISignalHolder
    {
        public BuildingsSignalsIncoming Incoming = new();
        public BuildingsSignalsOutgoing Outgoing = new();
    }

    public class BuildingsSignalsIncoming
    {
        /// <summary>
        /// Preview a building of this type on the free area nearest the board's centre - or at the
        /// centre, red, announcing NoFreeArea, when it fits nowhere - and wait for the player: pressing
        /// the board moves it, the green tick places it there (its cells become occupied), the red cross
        /// drops it. A new pick while one is waiting replaces it.
        /// </summary>
        public Signal<BuildType> PlaceBuilding = new();

        /// <summary>
        /// A press began on the world here (world units). While a building waits to be placed it moves
        /// that building; otherwise it selects the building under it, or clears the selection.
        /// </summary>
        public Signal<Vector2> PointerPressed = new();

        /// <summary>The press goes on and the pointer has moved here. Kept out of the Flow Console - it fires every frame of a drag.</summary>
        public Signal<Vector2> PointerDragged = new(hideCommandLog: true);

        /// <summary>The press ended here.</summary>
        public Signal<Vector2> PointerReleased = new();

        /// <summary>The selected building produces a unit of this type; it walks out of the building's door.</summary>
        public Signal<UnitType> ProduceUnit = new();
    }

    public class BuildingsSignalsOutgoing
    {
        /// <summary>A picked building's footprint (this size, in cells) fits nowhere on the board; it starts at the centre, red.</summary>
        public Signal<Vector2Int> NoFreeArea = new();

        /// <summary>A placed building asks for a unit: which one, the door it comes out of and where it walks to.</summary>
        public Signal<UnitSpawnRequestVO> UnitRequested = new();

        /// <summary>The player selected a building on the board (or pressed the selected one again).</summary>
        public Signal<BuildingInfoVO> BuildingSelected = new();

        /// <summary>The player pressed away from the selected building; nothing is selected any more.</summary>
        public Signal SelectionCleared = new();
    }
}
