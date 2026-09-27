using FlowIoC.BaseModule.Controller;
using FlowIoC.BaseModule.Injectable.Attributes;
using Modules.GridModule.Services;
using Modules.GridModule.Shared.Data.ValueObjects;
using Modules.GridModule.Signals;
using UnityEngine;

namespace Modules.GridModule.Controllers
{
    /// <summary>
    /// A strike landed on something on the board: its health drops by the damage, never below 0. Still standing, it
    /// is announced as damaged; at 0 it is taken off the board - every cell it covered is free again - and announced
    /// as destroyed, so the module that shows it can put it away.
    /// </summary>
    internal class DamageOccupantCommand : Command
    {
        [Inject]       private IGridService   _gridService { get; set; }
        [InjectSignal] private GridSignals    _signals     { get; set; }
        [SignalParam]  private CellOccupantVO _occupant    { get; set; }
        [SignalParam]  private int            _damage      { get; set; }

        public override void Execute()
        {
            _occupant.Hp = Mathf.Max(0, _occupant.Hp - _damage);

            if (_occupant.Hp > 0)
            {
                AnnounceDamaged();
                return;
            }

            _gridService.Remove(_occupant);
            AnnounceDestroyed();
        }

        private void AnnounceDamaged()
        {
            switch (_occupant)
            {
                case BoardBuildingVO building:
                    _signals.Outgoing.BuildingDamaged.Dispatch(building);
                    break;
                case BoardUnitVO unit:
                    _signals.Outgoing.UnitDamaged.Dispatch(unit);
                    break;
            }
        }

        private void AnnounceDestroyed()
        {
            switch (_occupant)
            {
                case BoardBuildingVO building:
                    _signals.Outgoing.BuildingDestroyed.Dispatch(building);
                    break;
                case BoardUnitVO unit:
                    _signals.Outgoing.UnitDestroyed.Dispatch(unit);
                    break;
            }
        }
    }
}
