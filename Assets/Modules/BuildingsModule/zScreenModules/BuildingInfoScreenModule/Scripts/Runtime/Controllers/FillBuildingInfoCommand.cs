using System.Collections.Generic;
using FlowIoC.BaseModule.Controller;
using FlowIoC.BaseModule.Injectable.Attributes;
using FlowIoC.PoolModule.Services;
using Modules.BuildingsModule.BuildingInfoScreenModule.Data.ValueObjects;
using Modules.BuildingsModule.BuildingInfoScreenModule.Entities;
using Modules.BuildingsModule.BuildingInfoScreenModule.Models;
using Modules.BuildingsModule.BuildingInfoScreenModule.ViewsMediators;
using Modules.BuildingsModule.Shared.Data.ValueObjects;
using Modules.UnitsModule.Shared.Enums;

namespace Modules.BuildingsModule.BuildingInfoScreenModule.Controllers
{
    /// <summary>
    /// Fills the screen the step before handed over with the shown building: its image, name and health
    /// out of what CD_Buildings authors, and a card per unit it produces. The last building's cards go
    /// back to the pool (group "building_info", warmed while the game loaded) and the new ones come out
    /// of it. Everything on the screen changes together, so it is set in one go rather than by a signal
    /// per value.
    /// </summary>
    internal class FillBuildingInfoCommand : Command<BuildingInfoScreenView>
    {
        /// <summary>The key of the card in CD_PoolGroup_BuildingInfo.</summary>
        private const string UnitItemPoolKey = "unit_item";

        [Inject]      private IBuildingInfoModel _buildingInfoModel { get; set; }
        [Inject]      private IPoolService       _poolService       { get; set; }
        [SignalParam] private BuildingInfoVO     _info              { get; set; }

        public override void Execute(BuildingInfoScreenView screen)
        {
            foreach (UnitItem card in screen.RemoveUnits())
                _poolService.Return.Item(card);

            BuildingCVO building = _buildingInfoModel.Buildings[_info.Type];
            var units = new List<UnitCardVO>(building.ProducibleUnits.Count);

            foreach (UnitType unit in building.ProducibleUnits)
                units.Add(new UnitCardVO(_poolService.Get<UnitItem>(UnitItemPoolKey, null), unit, _buildingInfoModel.Units[unit].Sprite));

            screen.ShowBuilding(_info.Type.ToString(), building.Icon, _info.Hp, building.Hp, units);
        }
    }
}
