using Modules.GridModule.Shared.Data.ValueObjects;

namespace Modules.UnitsModule.Models
{
    internal class UnitSelectionModel : IUnitSelectionModel
    {
        public BoardUnitVO Selected { get; private set; }

        public void Select(BoardUnitVO unit) => Selected = unit;

        public void ClearSelection() => Selected = null;
    }
}
