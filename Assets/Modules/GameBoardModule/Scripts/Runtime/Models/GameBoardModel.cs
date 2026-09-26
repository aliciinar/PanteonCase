using FlowIoC.BaseModule.Adapters;
using FlowIoC.BaseModule.Constructables;
using FlowIoC.BaseModule.Injectable.Attributes;
using Modules.GameBoardModule.Data.UnityObjects;
using Modules.GameBoardModule.Data.ValueObjects;
using Modules.GameBoardModule.RootsContexts;
using UnityEngine;

namespace Modules.GameBoardModule.Models
{
    /// <summary>
    /// Reads CD_GameBoard off the Root's adapter in PostConstruct and hands out what the board is made
    /// of. Laying the board out is BuildGameBoardCommand's work; the cells themselves are the grid
    /// service's.
    /// </summary>
    public class GameBoardModel : IGameBoardModel, IConstructable
    {
        [Inject(nameof(GameBoardSystemContext))]
        private GameObject _root { get; set; }

        public bool IsPostConstructed { get; set; }
        public bool IsDeconstructed { get; set; }

        public Vector2Int GridSize { get; private set; }
        public float CellSize { get; private set; }
        public float FramePaddingInCells { get; private set; }

        // detayına bakılacak.
        public void PostConstruct()
        {
            GameBoardCVO board = _root.GetComponent<RootAdapter>().GetScriptable<CD_GameBoard>().Board;

            GridSize = board.GridSize;
            CellSize = board.CellPixelSize / board.PixelsPerUnit;
            FramePaddingInCells = board.FramePaddingInCells;
        }

        public void Deconstruct()
        {
        }
    }
}
