using System.Collections.Generic;
using FlowIoC.BaseModule.Controller;
using FlowIoC.BaseModule.Injectable.Attributes;
using FlowIoC.PoolModule.Services;
using Modules.BuildingsModule.Shared.Data.ValueObjects;
using Modules.GameplayModule.InfoScreenModule.Data.ValueObjects;
using Modules.GameplayModule.InfoScreenModule.Entities;
using Modules.GameplayModule.InfoScreenModule.Enums;
using Modules.GameplayModule.InfoScreenModule.Models;
using Modules.GameplayModule.InfoScreenModule.ViewsMediators;
using Modules.UnitsModule.Shared.Data.ValueObjects;
using Modules.UnitsModule.Shared.Enums;

namespace Modules.GameplayModule.InfoScreenModule.Controllers
{
    /// <summary>
    /// Fills the screen the step before handed over with the shown building: its image, name and health
    /// out of what CD_Buildings authors, and a card per unit it produces, each carrying the request it
    /// makes when clicked - the unit, the building's door and spawn point. The last building's cards go
    /// back to the pool (group "building_info", warmed while the game loaded) and the new ones come out
    /// of it. Everything on the screen changes together, so it is set in one go rather than by a signal
    /// per value.
    /// </summary>
    internal class FillBuildingInfoCommand : Command<InfoScreenView>
    {
        /// <summary>The key of the card in CD_PoolGroup_BuildingInfo.</summary>
        private const string UnitItemPoolKey = "unit_item";

        [Inject]      private IInfoScreenModel _infoScreenModel { get; set; }
        [Inject]      private IPoolService     _poolService     { get; set; }
        [SignalParam] private BuildingInfoVO   _info            { get; set; }

        public override void Execute(InfoScreenView screen)
        {
            foreach (UnitItem card in screen.RemoveUnits())
                _poolService.Return.Item(card);

            BuildingCVO building = _infoScreenModel.Buildings[_info.Type];
            var units = new List<UnitCardVO>(building.ProducibleUnits.Count);

            foreach (UnitType unit in building.ProducibleUnits)
            {
                UnitCVO config = _infoScreenModel.Units[unit];
                units.Add(new UnitCardVO(_poolService.Get<UnitItem>(UnitItemPoolKey, null),
                                         new UnitSpawnRequestVO(unit, _info.ExitCell, _info.SpawnCell),
                                         config.Sprite, config.Name));
            }

            screen.ShowBuilding(building.Name, building.Icon, _info.Hp, building.Hp, units);
            _infoScreenModel.SetShown(InfoSubjectType.Building);
        }
    }
}
