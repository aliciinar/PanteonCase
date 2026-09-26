using FlowIoC.BaseModule.Signals;
using Modules.UnitsModule.Shared.Data.ValueObjects;
using Modules.UnitsModule.Shared.Enums;

namespace Modules.UnitsModule.Signals
{
    public class UnitsSignals : ISignalHolder
    {
        public UnitsSignalsIncoming Incoming = new();
        public UnitsSignalsOutgoing Outgoing = new();
    }

    public class UnitsSignalsIncoming
    {
        /// <summary>
        /// Put a unit on the board for a building: it comes out of the building's exit cell - its door -
        /// and walks by A* to the spawn cell, or, when that is taken or off the grid, the free cell nearest
        /// it. Answered with NoRoomForUnit when no cell is free.
        /// </summary>
        public Signal<UnitSpawnRequestVO> SpawnUnit = new();
    }

    public class UnitsSignalsOutgoing
    {
        /// <summary>A unit of this type was asked for, but no cell of the board is free.</summary>
        public Signal<UnitType> NoRoomForUnit = new();
    }
}
