using FlowIoC.BaseModule.Signals;
using Modules.GridModule.Shared.Data.ValueObjects;
using UnityEngine;

namespace Modules.GridModule.Signals
{
    public class GridSignals : ISignalHolder
    {
        public GridSignalsIncoming Incoming = new();
        public GridSignalsOutgoing Outgoing = new();
    }

    public class GridSignalsIncoming
    {
        /// <summary>A press began on the world here (world units): say what it landed on.</summary>
        public Signal<Vector2> PointerPressed = new();
    }

    /// <summary>What a press on the board landed on - exactly one of these per press.</summary>
    public class GridSignalsOutgoing
    {
        /// <summary>A building was pressed: the one standing on the pressed cell, as the grid holds it.</summary>
        public Signal<BoardBuildingVO> BuildingPressed = new();

        /// <summary>A unit was pressed: the one holding the pressed cell, as the grid holds it.</summary>
        public Signal<BoardUnitVO> UnitPressed = new();

        /// <summary>A free cell, or somewhere off the grid, was pressed.</summary>
        public Signal EmptyPressed = new();
    }
}
