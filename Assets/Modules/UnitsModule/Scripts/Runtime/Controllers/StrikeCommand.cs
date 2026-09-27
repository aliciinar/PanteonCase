using FlowIoC.BaseModule.Controller;
using FlowIoC.BaseModule.Injectable.Attributes;
using Modules.UnitsModule.Data.ValueObjects;
using Modules.UnitsModule.Models;
using Modules.UnitsModule.Signals;

namespace Modules.UnitsModule.Controllers
{
    /// <summary>
    /// The attacker's lunge reached its target: the strike lands for the attacker's damage (CD_Units). What it does to
    /// the target's health is the grid's business - it keeps the health.
    /// </summary>
    internal class StrikeCommand : Command
    {
        [Inject]       private IUnitsModel  _unitsModel { get; set; }
        [InjectSignal] private UnitsSignals _signals    { get; set; }
        [SignalParam]  private UnitStrikeVO _strike     { get; set; }

        public override void Execute() =>
            _signals.Outgoing.AttackLanded.Dispatch(_strike.Target, _unitsModel.Units[_strike.Attacker.Type].Damage);
    }
}
