using FlowIoC.BaseModule.Signals;
using Modules.BuildingsModule.Shared.Data.ValueObjects;
using Modules.BuildingsModule.Shared.Enums;
using Modules.GridModule.Shared.Data.ValueObjects;
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
        /// centre, red, when it fits nowhere - and wait for the player: pressing
        /// the board moves it, the green tick places it there (its cells become occupied), the red cross
        /// drops it. A new pick while one is waiting replaces it.
        /// </summary>
        public Signal<BuildType> PlaceBuilding = new();

        /// <summary>
        /// A press began on the world here (world units). Heard only while a building waits to be placed:
        /// it moves that building. What a press selects comes from the grid, through SelectBuilding and
        /// ClearSelection.
        /// </summary>
        public Signal<Vector2> PointerPressed = new();

        /// <summary>The press goes on and the pointer has moved here. Kept out of the Flow Console - it fires every frame of a drag.</summary>
        public Signal<Vector2> PointerDragged = new(hideCommandLog: true);

        /// <summary>The press ended here.</summary>
        public Signal<Vector2> PointerReleased = new();

        /// <summary>A building on the board was pressed: select it.</summary>
        public Signal<BoardBuildingVO> SelectBuilding = new();

        /// <summary>Something other than a building was pressed: nothing is selected.</summary>
        public Signal ClearSelection = new();

    }

    public class BuildingsSignalsOutgoing
    {
        /// <summary>
        /// The player selected a building on the board (or pressed the selected one again): which one, its
        /// health, and the grid cells its units come out of and walk to.
        /// </summary>
        public Signal<BuildingInfoVO> BuildingSelected = new();

        /// <summary>The player pressed away from the selected building; nothing is selected any more.</summary>
        public Signal SelectionCleared = new();

        /// <summary>
        /// A building is being placed: its ghost waits on the board and presses move it, so a press's drags are wanted
        /// and nothing else may take a press until PlacementEnded.
        /// </summary>
        public Signal PlacementStarted = new();

        /// <summary>The placement was confirmed or cancelled: nothing needs a press's drags any more, and presses are free again.</summary>
        public Signal PlacementEnded = new();
    }
}
