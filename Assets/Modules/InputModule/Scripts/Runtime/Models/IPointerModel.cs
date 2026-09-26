namespace Modules.InputModule.Models
{
    /// <summary>The state of the current pointer press.</summary>
    public interface IPointerModel
    {
        /// <summary>
        /// Whether the current press started on the game world. A press that started over UI belongs
        /// to the UI, and none of it - drag or release - is announced.
        /// </summary>
        bool IsPressOnWorld { get; set; }
    }
}
