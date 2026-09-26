using System;
using FlowIoC.BaseModule.Injectable.Components;
using FlowIoC.BaseModule.ViewsMediators.View;
using Modules.BuildingsModule.Shared.Enums;
using Modules.GameBoardModule.Entities;
using Modules.GameBoardModule.Shared.Data.ValueObjects;
using UnityEngine;
using UnityEngine.UI;

namespace Modules.GameBoardModule.ViewsMediators
{
    /// <summary>
    /// The board in the scene: a 9-sliced frame and a tiled grid, two sprite renderers in total so
    /// the whole board costs next to nothing in draw calls, the buildings standing on it, and the
    /// preview of a placement waiting for the player - a ghost of the building with a confirm / cancel
    /// prompt, a world-space canvas that moves and zooms with the board. Sizes and positions come from
    /// the model.
    /// </summary>
    [RequireComponent(typeof(ViewInjector))]
    public class GameBoardView : MonoBehaviour, IView
    {
        public bool IsRegistered { get; set; }

        [SerializeField] private SpriteRenderer _frame;
        [SerializeField] private SpriteRenderer _grid;

        [Tooltip("Parent of the buildings standing on the board.")]
        [SerializeField] private Transform _buildings;

        [Header("Placement preview")]
        [Tooltip("Parent of the ghost and the prompt; off while no placement is waiting.")]
        [SerializeField] private GameObject _placementPreview;

        [SerializeField] private BoardBuilding _ghost;

        [Tooltip("World-space canvas holding the two buttons, authored in cells (one canvas unit = one cell).")]
        [SerializeField] private RectTransform _prompt;

        [SerializeField] private Button _confirmButton;
        [SerializeField] private Button _cancelButton;

        public event Action PlacementConfirmClicked;
        public event Action PlacementCancelClicked;

        /// <summary>The cells last drawn, indexed [column, row]. Empty until the board is built.</summary>
        internal CellVO[,] Cells { get; private set; } = new CellVO[0, 0];

        /// <summary>Edge of one cell in world units.</summary>
        internal float CellSize { get; private set; }

        private void Awake()
        {
            _confirmButton.onClick.AddListener(() => PlacementConfirmClicked?.Invoke());
            _cancelButton.onClick.AddListener(() => PlacementCancelClicked?.Invoke());
        }

        /// <param name="gridBounds">World rect the cells cover.</param>
        /// <param name="frameBounds">World rect the frame fills.</param>
        /// <param name="cellSize">Edge of one cell in world units; the grid sprite tiles once per cell.</param>
        /// <param name="cells">Every cell, indexed [column, row].</param>
        public void Draw(Rect gridBounds, Rect frameBounds, float cellSize, CellVO[,] cells)
        {
            Cells = cells;
            CellSize = cellSize;

            _frame.drawMode = SpriteDrawMode.Sliced;
            _frame.transform.position = frameBounds.center;
            _frame.size = frameBounds.size;

            // The grid sprite is one cell at scale 1, so the renderer is scaled to the cell size and
            // sized in cells - which keeps one tile per cell whatever CellPixelSize says.
            _grid.drawMode = SpriteDrawMode.Tiled;
            _grid.transform.position = gridBounds.center;
            _grid.transform.localScale = Vector3.one * cellSize;
            _grid.size = gridBounds.size / cellSize;
        }

        /// <param name="building">A pooled building, taken out of the pool for this.</param>
        /// <param name="type">Which building it is; names it in the Hierarchy.</param>
        /// <param name="sprite">What the building looks like.</param>
        /// <param name="area">World rect the building covers - its footprint on the board.</param>
        public void PlaceBuilding(BoardBuilding building, BuildType type, Sprite sprite, Rect area)
        {
            building.name = type.ToString();
            building.transform.SetParent(_buildings, false);
            building.Show(sprite, area);
        }

        /// <param name="sprite">The building the ghost shows.</param>
        /// <param name="area">World rect the building would cover.</param>
        /// <param name="promptCentre">World point the confirm / cancel prompt is centred on.</param>
        public void ShowPlacementPreview(Sprite sprite, Rect area, Vector2 promptCentre)
        {
            _ghost.Show(sprite, area);

            _prompt.position = promptCentre;
            _prompt.localScale = Vector3.one * CellSize;

            _placementPreview.SetActive(true);
        }

        public void HidePlacementPreview() => _placementPreview.SetActive(false);
    }
}
