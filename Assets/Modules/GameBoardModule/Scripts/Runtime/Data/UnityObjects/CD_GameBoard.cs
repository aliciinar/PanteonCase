using Modules.GameBoardModule.Data.ValueObjects;
using UnityEngine;

namespace Modules.GameBoardModule.Data.UnityObjects
{
    /// <summary>The board's config, filed on GameBoardSystemRoot's RootAdapter and read once by GameBoardModel.</summary>
    [CreateAssetMenu(fileName = "CD_GameBoard", menuName = "Game/Data/CD_GameBoard")]
    internal class CD_GameBoard : ScriptableObject
    {
        public GameBoardCVO Board = new();
    }
}
