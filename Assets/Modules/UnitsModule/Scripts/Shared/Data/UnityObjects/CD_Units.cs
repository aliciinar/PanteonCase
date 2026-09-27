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
    }
}
