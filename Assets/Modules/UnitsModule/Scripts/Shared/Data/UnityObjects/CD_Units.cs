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

        [Tooltip("The colour a selected unit's sprite wears. A vertex colour, so coloured units still batch.")]
        public Color SelectedColor = new(1f, 0.85f, 0.3f, 1f);

        [Tooltip("How a struck unit flashes.")]
        public HitFlashCVO HitFlash = new();

        [Tooltip("The puff a destroyed unit leaves.")]
        public UnitExplosionCVO Explosion = new();

        [Tooltip("Told the player when the selected unit has no way to where it was ordered, or to any free cell next to what it was ordered to attack - buildings wall it off.")]
        [TextArea] public string NoWayMessage = "The soldier has no way there.";

        [Tooltip("Told the player when a requested unit can walk out of its building's door to no free cell.")]
        [TextArea] public string NoRoomToSpawnMessage = "There is no free cell the soldier can walk to from the door.";
    }
}
