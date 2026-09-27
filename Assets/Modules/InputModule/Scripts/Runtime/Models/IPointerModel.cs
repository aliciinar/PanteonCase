namespace Modules.InputModule.Models
{
    /// <summary>The state of the current pointer press. It changes only through the steps below.</summary>
    internal interface IPointerModel
    {
        /// <summary>
        /// Whether the current press started on the game world. A press that started over UI belongs
        /// to the UI, and none of it - drag or release - is announced.
        /// </summary>
        bool IsPressOnWorld { get; }

        /// <summary>A press started - on the world, or over UI.</summary>
        void BeginPress(bool onWorld);

        void EndPress();
    }
}
