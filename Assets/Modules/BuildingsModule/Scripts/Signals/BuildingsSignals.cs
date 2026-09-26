using FlowIoC.BaseModule.Signals;
using Modules.BuildingsModule.Shared.Enums;
using Modules.UnitsModule.Shared.Data.ValueObjects;
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

        /// <summary>A press began on the world here (world units).</summary>
        public Signal<Vector2> PointerPressed = new();

        /// <summary>The press goes on and the pointer has moved here. Kept out of the Flow Console - it fires every frame of a drag.</summary>
        public Signal<Vector2> PointerDragged = new(hideCommandLog: true);

        /// <summary>The press ended here.</summary>
        public Signal<Vector2> PointerReleased = new();
    }

    public class BuildingsSignalsOutgoing
    {
        /// <summary>A picked building's footprint (this size, in cells) fits nowhere on the board; it starts at the centre, red.</summary>
        public Signal<Vector2Int> NoFreeArea = new();

        /// <summary>A placed building asks for a unit: which one, the door it comes out of and where it walks to.</summary>
        public Signal<UnitSpawnRequestVO> UnitRequested = new();
    }
}
