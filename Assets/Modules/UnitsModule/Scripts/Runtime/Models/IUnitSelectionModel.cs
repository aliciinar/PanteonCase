using Modules.GridModule.Shared.Data.ValueObjects;

namespace Modules.UnitsModule.Models
{
    /// <summary>
    /// The unit the player selected with a press, kept until a later secondary press orders it somewhere - two
    /// presses apart, so it has to be kept. It holds the grid's own instance, never a copy.
    /// </summary>
    internal interface IUnitSelectionModel
    {
        /// <summary>The selected unit; null when none is.</summary>
        BoardUnitVO Selected { get; }

        void Select(BoardUnitVO unit);

        void ClearSelection();
    }
}
