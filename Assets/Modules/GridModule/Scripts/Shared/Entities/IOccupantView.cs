namespace Modules.GridModule.Shared.Entities
{
    /// <summary>
    /// The object that shows something standing on the board - a unit's, a building's. The grid keeps it with the
    /// thing's data (CellOccupantVO.View) and knows nothing more of it: the module that made the object is the one
    /// that works on it, reaching it through the cell the thing stands on.
    /// </summary>
    public interface IOccupantView
    {
    }
}
