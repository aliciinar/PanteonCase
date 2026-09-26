using Modules.BuildingsModule.Shared.Enums;
using UnityEngine;
using UnityEngine.Rendering;

namespace Modules.BuildingsModule.ProductionMenuScreenModule.Data.UnityObjects
{
    /// <summary>
    /// What the production menu offers, in order, and the sprite each building is shown with. Filed on
    /// BuildingsSystemRoot's RootAdapter and read by ProductionMenuModel.
    /// </summary>
    [CreateAssetMenu(fileName = "CD_ProductionMenu", menuName = "Game/Data/CD_ProductionMenu")]
    internal class CD_ProductionMenu : ScriptableObject
    {
        public SerializedDictionary<BuildType, Sprite> Sprites = new();

        [Tooltip("Cards per row of the menu.")]
        [Min(1)] public int Columns = 2;
    }
}
