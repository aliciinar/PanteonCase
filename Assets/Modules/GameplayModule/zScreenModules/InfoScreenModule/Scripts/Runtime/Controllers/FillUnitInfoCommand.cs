using FlowIoC.BaseModule.Controller;
using FlowIoC.BaseModule.Injectable.Attributes;
using FlowIoC.PoolModule.Services;
using Modules.GameplayModule.InfoScreenModule.Entities;
using Modules.GameplayModule.InfoScreenModule.Enums;
using Modules.GameplayModule.InfoScreenModule.Models;
using Modules.GameplayModule.InfoScreenModule.ViewsMediators;
using Modules.UnitsModule.Shared.Data.ValueObjects;

namespace Modules.GameplayModule.InfoScreenModule.Controllers
{
    /// <summary>
    /// Fills the screen the step before handed over with the shown unit: its image, name, health and damage out of
    /// what CD_Units authors. A unit produces nothing, so the cards of a building shown before go back to the pool
    /// and the production section is hidden.
    /// </summary>
    internal class FillUnitInfoCommand : Command<InfoScreenView>
    {
        [Inject]      private IInfoScreenModel _infoScreenModel { get; set; }
        [Inject]      private IPoolService     _poolService     { get; set; }
        [SignalParam] private UnitInfoVO       _info            { get; set; }

        public override void Execute(InfoScreenView screen)
        {
            foreach (UnitItem card in screen.RemoveUnits())
                _poolService.Return.Item(card);

            UnitCVO unit = _infoScreenModel.Units[_info.Type];
            screen.ShowUnit(unit.Name, unit.Sprite, _info.Hp, unit.Hp, unit.Damage);
            _infoScreenModel.SetShown(InfoSubjectType.Unit);
        }
    }
}
