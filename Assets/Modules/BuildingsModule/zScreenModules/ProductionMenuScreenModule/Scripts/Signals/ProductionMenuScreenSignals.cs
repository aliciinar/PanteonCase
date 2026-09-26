using FlowIoC.BaseModule.Signals;
using Modules.BuildingsModule.Shared.Enums;
using UnityEngine;

namespace Modules.BuildingsModule.ProductionMenuScreenModule.Signals
{
    public class ProductionMenuScreenSignals : ISignalHolder
    {
        public ProductionMenuScreenSignalsIncoming Incoming = new();
        public ProductionMenuScreenSignalsOutgoing Outgoing = new();
    }

    public class ProductionMenuScreenSignalsIncoming
    {
        public Signal OpenProductionMenuScreen = new();

        /// <summary>The window changed size (new size in pixels): lay the menu out again.</summary>
        public Signal<Vector2Int> ScreenResized = new();
    }

    public class ProductionMenuScreenSignalsOutgoing
    {
        /// <summary>
        /// The screen rect the menu panel covers, normalised (0-1, origin bottom-left). Sent when the
        /// screen shows and whenever the window is resized.
        /// </summary>
        public Signal<Rect> AreaChanged = new();

        /// <summary>The player picked a building in the menu.</summary>
        public Signal<BuildType> BuildTypeSelected = new();
    }
}
