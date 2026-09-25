using FlowIoC.BaseModule.Signals;
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
    }

    public class GameBoardSignalsOutgoing
    {
        /// <summary>
        /// The board has been laid out. Carries the world rect it occupies, frame included - the area
        /// whoever frames the board has to keep in view.
        /// </summary>
        public Signal<Rect> BoardBuilt = new();
    }
}
