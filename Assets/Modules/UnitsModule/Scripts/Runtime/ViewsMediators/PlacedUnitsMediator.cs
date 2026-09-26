using FlowIoC.BaseModule.Injectable.Attributes;
using FlowIoC.BaseModule.ViewsMediators.Mediator;
using Modules.UnitsModule.Data.ValueObjects;
using Modules.UnitsModule.Signals;

namespace Modules.UnitsModule.ViewsMediators
{
    public class PlacedUnitsMediator : IMediator
    {
        [Inject]       private PlacedUnitsView      _view            { get; set; }
        [InjectSignal] private UnitsInternalSignals _internalSignals { get; set; }

        public void OnRegister() => _internalSignals.ShowUnit.AddListener(OnShowUnit);

        public void OnRemove() => _internalSignals.ShowUnit.RemoveListener(OnShowUnit);

        private void OnShowUnit(PlacedUnitVO unit) =>
            _view.PlaceUnit(unit.Unit, unit.Type, unit.Sprite, unit.SpawnArea, unit.Waypoints, unit.Speed);
    }
}
