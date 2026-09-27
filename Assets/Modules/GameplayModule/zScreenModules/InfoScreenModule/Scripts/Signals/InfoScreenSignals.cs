using FlowIoC.BaseModule.Signals;
using Modules.BuildingsModule.Shared.Data.ValueObjects;
using Modules.UnitsModule.Shared.Data.ValueObjects;

namespace Modules.GameplayModule.InfoScreenModule.Signals
{
    public class InfoScreenSignals : ISignalHolder
    {
        public InfoScreenSignalsIncoming Incoming = new();
        public InfoScreenSignalsOutgoing Outgoing = new();
    }

    public class InfoScreenSignalsIncoming
    {
        /// <summary>
        /// Show this building inside the information panel: its image, name and health, and a card per
        /// unit it produces - each card asking for its unit at the building's door and spawn point. Opens
        /// the screen when it is closed and refills it when it is open.
        /// </summary>
        public Signal<BuildingInfoVO> ShowBuildingInfo = new();

        /// <summary>
        /// No building is selected any more: close the screen, leaving the bare information panel - only while it
        /// shows a building; a unit shown in the meantime stays.
        /// </summary>
        public Signal HideBuildingInfo = new();

        /// <summary>
        /// Show this unit inside the information panel: its image, name, health and damage. Opens the screen when it
        /// is closed and refills it when it is open.
        /// </summary>
        public Signal<UnitInfoVO> ShowUnitInfo = new();

        /// <summary>No unit is selected any more: close the screen - only while it shows a unit.</summary>
        public Signal HideUnitInfo = new();
    }

    public class InfoScreenSignalsOutgoing
    {
        /// <summary>The player clicked a unit card: the shown building produces that unit - out of its door, walking to its spawn point.</summary>
        public Signal<UnitSpawnRequestVO> UnitRequested = new();
    }
}
