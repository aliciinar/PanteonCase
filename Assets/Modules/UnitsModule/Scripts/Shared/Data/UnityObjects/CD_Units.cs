using Modules.UnitsModule.Shared.Data.ValueObjects;
using Modules.UnitsModule.Shared.Enums;
using UnityEngine;
using UnityEngine.Rendering;

namespace Modules.UnitsModule.Shared.Data.UnityObjects
{
    /// <summary>
    /// Every unit of the game, the one place a unit is defined. Filed in the Shared Scriptables of
    /// UnitsSystemRoot's RootAdapter, since other modules show units too - the information panel lists
    /// what a barracks produces.
    /// </summary>
    [CreateAssetMenu(fileName = "CD_Units", menuName = "Game/Data/CD_Units")]
    public class CD_Units : ScriptableObject
    {
        public SerializedDictionary<UnitType, UnitCVO> Units = new();

        [Tooltip("The colour a selected unit's sprite is tinted with. A vertex colour, so tinted units still batch.")]
        public Color SelectedTint = new(1f, 0.85f, 0.3f, 1f);

        [Tooltip("The colour a struck unit's sprite flashes with.")]
        public Color HitFlash = new(1f, 0.35f, 0.35f, 1f);

        [Tooltip("Seconds a hit's flash takes, there and back.")]
        [Min(0.02f)] public float HitFlashDuration = 0.2f;

        [Header("Destroyed")]
        [Tooltip("A destroyed unit leaves a puff with one of these, picked at random.")]
        public Sprite[] ExplosionSprites;

        [Tooltip("How wide the puff grows, in cells.")]
        [Min(0.1f)] public float ExplosionSize = 1.6f;

        [Tooltip("Seconds the puff takes to grow and fade.")]
        [Min(0.05f)] public float ExplosionDuration = 0.4f;

        [Tooltip("Told the player when the selected unit is ordered to attack something with no free cell next to it to strike from.")]
        [TextArea] public string NoRoomToAttackMessage = "There is no free cell next to the target to attack from.";

        [Tooltip("Told the player when the selected unit has no way to where it was ordered - buildings wall it off.")]
        [TextArea] public string NoWayMessage = "The soldier has no way there.";
    }
}
