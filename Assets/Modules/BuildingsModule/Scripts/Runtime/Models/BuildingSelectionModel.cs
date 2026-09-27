using Modules.GridModule.Shared.Data.ValueObjects;

namespace Modules.BuildingsModule.Models
{
    internal class BuildingSelectionModel : IBuildingSelectionModel
    {
        public BoardBuildingVO Selected { get; private set; }

        public void Select(BoardBuildingVO building) => Selected = building;

        public void ClearSelection() => Selected = null;
    }
}
