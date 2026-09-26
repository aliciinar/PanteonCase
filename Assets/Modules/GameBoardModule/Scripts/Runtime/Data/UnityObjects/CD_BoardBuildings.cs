using Modules.BuildingsModule.Shared.Enums;
using Modules.GameBoardModule.Data.ValueObjects;
using UnityEngine;
using UnityEngine.Rendering;

namespace Modules.GameBoardModule.Data.UnityObjects
{
    /// <summary>
    /// Every building as the board sees it: its footprint in cells and its sprite. Filed on
    /// GameBoardSystemRoot's RootAdapter and read by GameBoardModel.
    /// </summary>
    [CreateAssetMenu(fileName = "CD_BoardBuildings", menuName = "Game/Data/CD_BoardBuildings")]
    internal class CD_BoardBuildings : ScriptableObject
    {
        public SerializedDictionary<BuildType, BoardBuildingCVO> Buildings = new();
    }
}
