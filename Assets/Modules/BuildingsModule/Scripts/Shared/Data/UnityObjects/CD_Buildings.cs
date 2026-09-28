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
        [Tooltip("The colour a selected building's sprite is tinted with on the board. A vertex colour, so it still batches.")]
        public Color SelectedTint = new(1f, 0.85f, 0.3f, 1f);

        [Tooltip("The colour a struck building's sprite flashes with.")]
        public Color HitFlash = new(1f, 0.35f, 0.35f, 1f);

        [Tooltip("Seconds a hit's flash takes, there and back.")]
        [Min(0.02f)] public float HitFlashDuration = 0.2f;

        [Tooltip("How much of the footprint's width the health bar spans.")]
        [Range(0.1f, 1f)] public float HealthBarWidth = 0.8f;

        [Tooltip("How far below the footprint's top edge the health bar sits, in cells.")]
        [Min(0f)] public float HealthBarInset = 0.2f;

        [Header("Destroyed")]
        [Tooltip("A destroyed building explodes in puffs of these, each picked at random.")]
        public Sprite[] ExplosionSprites;

        [Tooltip("How many puffs a destroyed building explodes in: the first at its centre, the rest over its footprint.")]
        [Min(1)] public int ExplosionCount = 3;

        [Tooltip("Each puff's width, as a share of the footprint's longer side.")]
        [Min(0.1f)] public float ExplosionSize = 0.9f;

        [Tooltip("Seconds between one puff and the next.")]
        [Min(0f)] public float ExplosionStagger = 0.08f;

        [Tooltip("Seconds a puff takes to grow and fade.")]
        [Min(0.05f)] public float ExplosionDuration = 0.5f;

        [Header("Placement")]
        [Tooltip("The ghost's colour where the building fits.")]
        public Color PlacementFitsTint = new(0.55f, 1f, 0.55f, 0.7f);

        [Tooltip("The ghost's colour where the building does not fit.")]
        public Color PlacementBlockedTint = new(1f, 0.4f, 0.4f, 0.7f);
    }
}
