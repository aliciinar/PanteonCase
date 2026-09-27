namespace Modules.GameplayModule.Models
{
    /// <summary>Where the game stands - RD_GameStatus, which every module reads. Only this model writes it.</summary>
    internal interface IGameStatusModel
    {
        bool IsLocked { get; }

        /// <summary>An action runs, or has ended: the game takes no order while it runs.</summary>
        void SetLocked(bool isLocked);
    }
}
