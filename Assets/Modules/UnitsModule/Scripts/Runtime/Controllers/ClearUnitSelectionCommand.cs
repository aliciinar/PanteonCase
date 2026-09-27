using FlowIoC.BaseModule.Controller;
using FlowIoC.BaseModule.Function.Provider;
using FlowIoC.BaseModule.Injectable.Attributes;
using Modules.GridModule.Shared.Data.ValueObjects;
using Modules.UnitsModule.Controllers.BoardUnits;
using Modules.UnitsModule.Entities;
using Modules.UnitsModule.Models;
using UnityEngine;

namespace Modules.UnitsModule.Controllers
{
    /// <summary>
    /// Something other than a unit was pressed: no unit is selected any more, and the object of the one that was -
    /// reached through its data - wears white again.
    /// </summary>
    internal class ClearUnitSelectionCommand : Command
    {
        [Inject] private IUnitSelectionModel _selectionModel   { get; set; }
        [Inject] private IFunctionProvider   _functionProvider { get; set; }

        public override void Execute()
        {
            BoardUnitVO previous = _selectionModel.Selected;
            if (previous == null) return;

            _selectionModel.ClearSelection();
            _functionProvider.Call<TintBoardUnitFunction>().AddParams((BoardUnit)previous.View, Color.white).Execute();
        }
    }
}
