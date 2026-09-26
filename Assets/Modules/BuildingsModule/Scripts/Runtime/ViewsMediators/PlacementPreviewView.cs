using System;
using FlowIoC.BaseModule.Injectable.Components;
using FlowIoC.BaseModule.ViewsMediators.View;
using Modules.BuildingsModule.Entities;
using UnityEngine;
using UnityEngine.UI;

namespace Modules.BuildingsModule.ViewsMediators
{
    /// <summary>
    /// The preview of a placement waiting for the player: a ghost of the building, tinted by whether it
    /// fits, with a confirm / cancel prompt beside it - a world-space canvas that moves and zooms with
    /// the board. Both are hidden while nothing waits.
    /// </summary>
    [RequireComponent(typeof(ViewInjector))]
    public class PlacementPreviewView : MonoBehaviour, IView
    {
        public bool IsRegistered { get; set; }

        [SerializeField] private BuildingSprite _ghost;

        [Tooltip("The ghost's colour where the building fits.")]
        [SerializeField] private Color _fitsTint = new(0.55f, 1f, 0.55f, 0.7f);

        [Tooltip("The ghost's colour where the building does not fit.")]
        [SerializeField] private Color _blockedTint = new(1f, 0.4f, 0.4f, 0.7f);

        [Tooltip("World-space canvas holding the two buttons, authored in cells (one canvas unit = one cell).")]
        [SerializeField] private RectTransform _prompt;

        [SerializeField] private Button _confirmButton;
        [SerializeField] private Button _cancelButton;

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
        /// <param name="fits">Whether the building fits there: the ghost is tinted green or red, and only a fitting one can be confirmed.</param>
        /// <param name="cellSize">Edge of one cell in world units; the prompt is authored in cells.</param>
        public void Show(Sprite sprite, Rect area, Vector2 promptCentre, bool fits, float cellSize)
        {
            _ghost.Show(sprite, area);
            _ghost.Tint(fits ? _fitsTint : _blockedTint);
            _confirmButton.interactable = fits;

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
