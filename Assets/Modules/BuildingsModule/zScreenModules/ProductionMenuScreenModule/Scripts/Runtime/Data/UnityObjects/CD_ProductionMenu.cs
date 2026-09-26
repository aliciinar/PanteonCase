using UnityEngine;

namespace Modules.BuildingsModule.ProductionMenuScreenModule.Data.UnityObjects
{
    /// <summary>
    /// How the production menu is laid out. What it offers is every building in CD_Buildings, in that
    /// asset's order. Filed on BuildingsSystemRoot's RootAdapter and read by ProductionMenuModel.
    /// </summary>
    [CreateAssetMenu(fileName = "CD_ProductionMenu", menuName = "Game/Data/CD_ProductionMenu")]
    internal class CD_ProductionMenu : ScriptableObject
    {
        [Tooltip("Cards per row of the menu.")]
        [Min(1)] public int Columns = 2;
    }
}
