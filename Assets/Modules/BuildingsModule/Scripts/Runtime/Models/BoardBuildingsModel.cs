using System.Collections.Generic;
using Modules.BuildingsModule.Data.ValueObjects;

namespace Modules.BuildingsModule.Models
{
    internal class BoardBuildingsModel : IBoardBuildingsModel
    {
        public Dictionary<int, BuildingRecordVO> Buildings { get; } = new();
        public BuildingRecordVO Selected { get; set; }
    }
}
