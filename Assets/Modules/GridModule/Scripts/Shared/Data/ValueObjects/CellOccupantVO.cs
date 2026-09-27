using Modules.GridModule.Shared.Enums;

namespace Modules.GridModule.Shared.Data.ValueObjects
{
    /// <summary>
    /// What stands on a cell. The grid is the one place a thing on the board keeps its data: every cell an
    /// occupant covers holds the same instance, and whoever needs the thing reads it off the cell. Kind is
    /// what a search or a press asks first; the concrete type carries the rest.
    /// </summary>
    public abstract class CellOccupantVO
    {
        public abstract CellOccupantType Kind { get; }
    }
}
