using Modules.BuildingsModule.Shared.Data.ValueObjects;
using Modules.BuildingsModule.Shared.Enums;
using UnityEngine;
using UnityEngine.Rendering;

namespace Modules.BuildingsModule.Shared.Data.UnityObjects
{
    /// <summary>
    /// Every building of the game, the one place a building is defined, and how buildings look on the board - the
    /// selection, a hit, the health bar and the placement ghost. Filed in the Shared Scriptables of
    /// BuildingsSystemRoot's RootAdapter; the module reads it for placing buildings on the board, the production menu
    /// lists its entries in this order, and the info screen shows them.
    /// </summary>
    [CreateAssetMenu(fileName = "CD_Buildings", menuName = "Game/Data/CD_Buildings")]
    public class CD_Buildings : ScriptableObject
    {
        public SerializedDictionary<BuildType, BuildingCVO> Buildings = new();

        [Header("On the board")]
        [Tooltip("The colour a selected building's sprite wears on the board. A vertex colour, so it still batches.")]
        public Color SelectedColor = new(1f, 0.85f, 0.3f, 1f);

        [Tooltip("How a struck building flashes.")]
        public HitFlashCVO HitFlash = new();

        [Tooltip("How much of the footprint's width the health bar spans.")]
        [Range(0.1f, 1f)] public float HealthBarWidth = 0.8f;

        [Tooltip("How far below the footprint's top edge the health bar sits, in cells.")]
        [Min(0f)] public float HealthBarInset = 0.2f;

        [Header("Destroyed")]
        [Tooltip("How a destroyed building explodes.")]
        public BuildingExplosionCVO Explosion = new();

        [Header("Placement")]
        [Tooltip("The ghost's colour where the building fits.")]
        public Color PlacementFitsColor = new(0.55f, 1f, 0.55f, 0.7f);

        [Tooltip("The ghost's colour where the building does not fit.")]
        public Color PlacementBlockedColor = new(1f, 0.4f, 0.4f, 0.7f);
    }
}
