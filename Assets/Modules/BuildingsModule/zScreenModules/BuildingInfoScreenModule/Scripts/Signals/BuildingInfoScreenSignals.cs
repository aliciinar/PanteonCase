using FlowIoC.BaseModule.Signals;
using Modules.BuildingsModule.Shared.Data.ValueObjects;
using Modules.UnitsModule.Shared.Enums;

namespace Modules.BuildingsModule.BuildingInfoScreenModule.Signals
{
    public class BuildingInfoScreenSignals : ISignalHolder
    {
        public BuildingInfoScreenSignalsIncoming Incoming = new();
        public BuildingInfoScreenSignalsOutgoing Outgoing = new();
    }

    public class BuildingInfoScreenSignalsIncoming
    {
        /// <summary>
        /// Show this building inside the information panel: its image, name and health, and a card per
        /// unit it produces. Opens the screen when it is closed and refills it when it is open.
        /// </summary>
        public Signal<BuildingInfoVO> ShowBuildingInfo = new();

        /// <summary>Close the screen, leaving the bare information panel. Nothing happens when it is closed.</summary>
        public Signal HideBuildingInfo = new();
    }

    public class BuildingInfoScreenSignalsOutgoing
    {
        /// <summary>The player clicked the card of this unit: the shown building is to produce one.</summary>
        public Signal<UnitType> UnitClicked = new();
    }
}
