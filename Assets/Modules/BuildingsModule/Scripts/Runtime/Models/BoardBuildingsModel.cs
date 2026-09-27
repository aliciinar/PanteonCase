using System.Collections.Generic;
using Modules.BuildingsModule.Data.ValueObjects;

namespace Modules.BuildingsModule.Models
{
    internal class BoardBuildingsModel : IBoardBuildingsModel
    {
        public BuildingRecordVO Selected { get; private set; }

        private readonly Dictionary<int, BuildingRecordVO> _buildings = new();

        public void Add(BuildingRecordVO building) => _buildings.Add(building.Id, building);

        public BuildingRecordVO Get(int entityId) => _buildings[entityId];

        public void Select(BuildingRecordVO building) => Selected = building;

        public bool ClearSelection()
        {
            if (Selected == null) return false;

            Selected = null;
            return true;
        }
    }
}
