using System;
using FlowIoC.BaseModule.Injectable.Components;
using FlowIoC.BaseModule.ViewsMediators.View;
using Modules.BuildingsModule.Entities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Modules.BuildingsModule.ViewsMediators
{
    /// <summary>
    /// The preview of a placement waiting for the player: a ghost of the building, tinted by whether it
    /// fits, with a confirm / cancel prompt beside it - a world-space canvas that moves and zooms with
    /// the board - and the building's name and footprint written under the two buttons. Both are hidden
    /// while nothing waits.
    /// </summary>
    [RequireComponent(typeof(ViewInjector))]
    public class PlacementPreviewView : MonoBehaviour, IView
    {
        public bool IsRegistered { get; set; }

        [SerializeField] private BuildingSprite _ghost;

        [Tooltip("World-space canvas holding the two buttons, authored in cells (one canvas unit = one cell).")]
        [SerializeField] private RectTransform _prompt;

        [SerializeField] private Button _confirmButton;
        [SerializeField] private Button _cancelButton;

        [Tooltip("Under the two buttons: the building's name and its footprint in cells. Not a raycast target, so a press on the board under it still reaches the board.")]
        [SerializeField] private TMP_Text _label;

        public event Action ConfirmClicked;
        public event Action CancelClicked;

        private void Awake()
        {
            _confirmButton.onClick.AddListener(() => ConfirmClicked?.Invoke());
            _cancelButton.onClick.AddListener(() => CancelClicked?.Invoke());
        }

        /// <param name="sprite">The building the ghost shows.</param>
        /// <param name="area">World rect the building would cover.</param>
        /// <param name="promptCentre">World point the confirm / cancel prompt is centred on.</param>
        /// <param name="fits">Whether the building fits there: only a fitting one can be confirmed.</param>
        /// <param name="tint">The ghost's colour - CD_Buildings' fits or blocked tint.</param>
        /// <param name="cellSize">Edge of one cell in world units; the prompt is authored in cells.</param>
        /// <param name="buildingName">The building's name, as CD_Buildings gives it.</param>
        /// <param name="size">The building's footprint in cells.</param>
        public void Show(Sprite sprite, Rect area, Vector2 promptCentre, bool fits, Color tint, float cellSize,
                         string buildingName, Vector2Int size)
        {
            _ghost.Show(sprite, area);
            _ghost.Tint(tint);
            _confirmButton.interactable = fits;
            _label.text = $"{buildingName}  {size.x}×{size.y}";

            _prompt.position = promptCentre;
            _prompt.localScale = Vector3.one * cellSize;

            _ghost.gameObject.SetActive(true);
            _prompt.gameObject.SetActive(true);
        }

        public void Hide()
        {
            _ghost.gameObject.SetActive(false);
            _prompt.gameObject.SetActive(false);
        }
    }
}
