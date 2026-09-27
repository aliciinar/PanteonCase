using UnityEngine;

namespace Modules.BuildingsModule.ProductionMenuScreenModule.Data.UnityObjects
{
    /// <summary>
    /// How the production menu lays out its grid of cards. What it offers is every building in CD_Buildings, in
    /// that asset's order. Filed on BuildingsSystemRoot's RootAdapter and read by ProductionMenuModel.
    /// </summary>
    [CreateAssetMenu(fileName = "CD_ProductionMenu", menuName = "Game/Data/CD_ProductionMenu")]
    internal class CD_ProductionMenu : ScriptableObject
    {
        [Tooltip("Cards per row of the menu.")]
        [Min(1)] public int Columns = 2;

        [Tooltip("Size of one card, in canvas units.")]
        [Min(1)] public Vector2 CellSize = new(140f, 140f);

        [Tooltip("Gap between two cards across and down, in canvas units. Half of it sits above every row and half below.")]
        [Min(0)] public Vector2 Spacing = new(16f, 16f);
    }
}
