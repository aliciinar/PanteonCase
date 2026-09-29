using Modules.GridModule.Shared.Data.ValueObjects;

namespace Modules.BuildingsModule.Models
{
    /// <summary>
    /// The building the player selected, kept so that the next press - another building, or anything else - can take
    /// the selection colour off it: two presses apart, so it has to be kept. It holds the grid's own instance, never a
    /// copy.
    /// </summary>
    internal interface IBuildingSelectionModel
    {
        /// <summary>The selected building; null when none is.</summary>
        BoardBuildingVO Selected { get; }

        void Select(BoardBuildingVO building);

        void ClearSelection();
    }
}
