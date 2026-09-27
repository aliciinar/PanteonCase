using FlowIoC.BaseModule.Injectable.Attributes;
using FlowIoC.BaseModule.ViewsMediators.Mediator;
using Modules.GridModule.Shared.Data.ValueObjects;
using Modules.UnitsModule.Data.ValueObjects;
using Modules.UnitsModule.Signals;
using UnityEngine;

namespace Modules.UnitsModule.ViewsMediators
{
    public class PlacedUnitsMediator : IMediator
    {
        [Inject]       private PlacedUnitsView      _view            { get; set; }
        [InjectSignal] private UnitsInternalSignals _internalSignals { get; set; }

        public void OnRegister()
        {
            _internalSignals.ShowUnit.AddListener(OnShowUnit);
            _internalSignals.MoveUnit.AddListener(OnMoveUnit);
            _internalSignals.ShowUnitSelected.AddListener(OnShowUnitSelected);
            _internalSignals.HideUnitSelected.AddListener(OnHideUnitSelected);

            _view.UnitStepped += OnUnitStepped;
        }

        public void OnRemove()
        {
            _internalSignals.ShowUnit.RemoveListener(OnShowUnit);
            _internalSignals.MoveUnit.RemoveListener(OnMoveUnit);
            _internalSignals.ShowUnitSelected.RemoveListener(OnShowUnitSelected);
            _internalSignals.HideUnitSelected.RemoveListener(OnHideUnitSelected);

            _view.UnitStepped -= OnUnitStepped;
        }

        private void OnShowUnit(PlacedUnitVO unit) =>
            _view.PlaceUnit(unit.Unit, unit.View, unit.Sprite, unit.SpawnArea, unit.Waypoints, unit.Speed);

        private void OnMoveUnit(UnitWalkVO walk) => _view.MoveUnit(walk.Unit, walk.Waypoints, walk.Speed);

        private void OnShowUnitSelected(BoardUnitVO unit, Color tint) => _view.ShowSelected(unit, tint);

        private void OnHideUnitSelected(BoardUnitVO unit) => _view.ShowDeselected(unit);

        private void OnUnitStepped(BoardUnitVO unit, Vector3 point) => _internalSignals.UnitStepped.Dispatch(unit, point);
    }
}
