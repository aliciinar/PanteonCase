using FlowIoC.BaseModule.Attributes;
using FlowIoC.BaseModule.Controller;
using FlowIoC.BaseModule.Injectable.Attributes;
using Modules.GridModule.Services;
using Modules.GridModule.Shared.Data.ValueObjects;
using UnityEngine;

namespace Modules.UnitsModule.Controllers
{
    /// <summary>
    /// A walking unit started stepping into another cell: the grid is told, so a new order starts the unit's walk
    /// from where it is. Kept out of the Flow Console - it runs once per cell of every walk.
    /// </summary>
    [HideCommandLog]
    internal class TrackUnitStepCommand : Command
    {
        [Inject]      private IGridService _gridService { get; set; }
        [SignalParam] private BoardUnitVO  _unit        { get; set; }
        [SignalParam] private Vector3      _point       { get; set; }

        public override void Execute() => _gridService.StepUnit(_unit, _gridService.WorldToCell(_point));
    }
}
