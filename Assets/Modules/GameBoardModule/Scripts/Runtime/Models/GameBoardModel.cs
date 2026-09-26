using FlowIoC.BaseModule.Adapters;
using FlowIoC.BaseModule.Constructables;
using FlowIoC.BaseModule.Injectable.Attributes;
using Modules.GameBoardModule.Data.UnityObjects;
using Modules.GameBoardModule.Data.ValueObjects;
using Modules.GameBoardModule.RootsContexts;
using Modules.GameBoardModule.Shared.Data.ValueObjects;
using UnityEngine;

namespace Modules.GameBoardModule.Models
{
    /// <summary>
    /// Reads CD_GameBoard off the Root's adapter in PostConstruct and lays the grid out from it:
    /// the grid is centred on the world origin and every cell's centre is computed once, here.
    /// </summary>
    public class GameBoardModel : IGameBoardModel, IConstructable
    {
        [Inject(nameof(GameBoardSystemContext))]
        private GameObject _root { get; set; }

        public bool IsPostConstructed { get; set; }
        public bool IsDeconstructed { get; set; }

        public Vector2Int GridSize { get; private set; }
        public float CellSize { get; private set; }
        public Rect GridBounds { get; private set; }
        public Rect FrameBounds { get; private set; }
        public CellVO[,] Cells { get; private set; }

        // detayına bakılacak.
        public void PostConstruct()
        {
            GameBoardCVO board = _root.GetComponent<RootAdapter>().GetScriptable<CD_GameBoard>().Board;

            GridSize = board.GridSize;
            CellSize = board.CellPixelSize / board.PixelsPerUnit;

            Vector2 gridWorldSize = new Vector2(GridSize.x, GridSize.y) * CellSize;
            GridBounds = new Rect(-gridWorldSize * 0.5f, gridWorldSize);

            float padding = board.FramePaddingInCells * CellSize;
            FrameBounds = new Rect(GridBounds.xMin - padding, GridBounds.yMin - padding,
                GridBounds.width + padding * 2f, GridBounds.height + padding * 2f);

            Cells = CreateCells();
        }

        public void Deconstruct() => Cells = null;

        public Vector2Int WorldToCell(Vector3 worldPosition) =>
            new(Mathf.FloorToInt((worldPosition.x - GridBounds.xMin) / CellSize),
                Mathf.FloorToInt((worldPosition.y - GridBounds.yMin) / CellSize));

        public bool IsInside(Vector2Int cell) =>
            cell.x >= 0 && cell.y >= 0 && cell.x < GridSize.x && cell.y < GridSize.y;

        // detayına bakılacak.
        private CellVO[,] CreateCells()
        {
            var cells = new CellVO[GridSize.x, GridSize.y];

            for (int column = 0; column < GridSize.x; column++)
            {
                for (int row = 0; row < GridSize.y; row++)
                {
                    var centre = new Vector2(GridBounds.xMin + (column + 0.5f) * CellSize,
                        GridBounds.yMin + (row + 0.5f) * CellSize);
                    cells[column, row] = new CellVO(centre);
                }
            }
            

            return cells;
        }
    }
}