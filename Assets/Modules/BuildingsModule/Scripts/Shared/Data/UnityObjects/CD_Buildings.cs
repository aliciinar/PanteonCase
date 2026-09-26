using Modules.BuildingsModule.Shared.Data.ValueObjects;
using Modules.BuildingsModule.Shared.Enums;
using UnityEngine;
using UnityEngine.Rendering;

namespace Modules.BuildingsModule.Shared.Data.UnityObjects
{
    /// <summary>
    /// Every building of the game, the one place a building is defined. Filed on BuildingsSystemRoot's
    /// RootAdapter; the module reads it for placing buildings on the board, and the production menu -
    /// whose context sits on the same Root - lists its entries in this order.
    /// </summary>
    [CreateAssetMenu(fileName = "CD_Buildings", menuName = "Game/Data/CD_Buildings")]
    public class CD_Buildings : ScriptableObject
    {
        public SerializedDictionary<BuildType, BuildingCVO> Buildings = new();
    }
}
