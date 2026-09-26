using FlowIoC.BaseModule.Signals;
using Modules.BuildingsModule.Shared.Enums;
using UnityEngine;

namespace Modules.GameBoardModule.Signals
{
    public class GameBoardSignals : ISignalHolder
    {
        public GameBoardSignalsIncoming Incoming = new();
        public GameBoardSignalsOutgoing Outgoing = new();
    }

    public class GameBoardSignalsIncoming
    {
        /// <summary>Lay the board out, draw it and announce it.</summary>
        public Signal BuildBoard = new();

        /// <summary>
        /// Find where an area of this size (in cells) fits, as close to the board's centre as possible.
        /// Answered with FreeAreaFound or NoFreeArea.
        /// </summary>
        public Signal<Vector2Int> FindFreeArea = new();

        /// <summary>
        /// Preview a building of this type on the free area nearest the board's centre - or at the
        /// centre, red, announcing NoFreeArea, when it fits nowhere - and wait for the player: pressing
        /// the board moves it, the green tick places it there (its cells become occupied), the red cross
        /// drops it. A new pick while one is waiting replaces it.
        /// </summary>
        public Signal<BuildType> PlaceBuilding = new();

        /// <summary>A press began on the world here (world units).</summary>
        public Signal<Vector2> PointerPressed = new();

        /// <summary>The press goes on and the pointer has moved here.</summary>
        public Signal<Vector2> PointerDragged = new();

        /// <summary>The press ended here.</summary>
        public Signal<Vector2> PointerReleased = new();
    }

    public class GameBoardSignalsOutgoing
    {
        /// <summary>
        /// The board has been laid out. Carries the world rect it occupies, frame included - the area
        /// whoever frames the board has to keep in view.
        /// </summary>
        public Signal<Rect> BoardBuilt = new();

        /// <summary>An area of <c>size</c> cells fits with its bottom-left cell at <c>origin</c>.</summary>
        public Signal<Vector2Int, Vector2Int> FreeAreaFound = new();

        /// <summary>An area of this size fits nowhere on the board.</summary>
        public Signal<Vector2Int> NoFreeArea = new();
    }
}
