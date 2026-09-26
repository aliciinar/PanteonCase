using FlowIoC.BaseModule.Injectable.Attributes;
using FlowIoC.BaseModule.ViewsMediators.Mediator;
using Modules.BuildingsModule.Data.ValueObjects;
using Modules.BuildingsModule.Signals;

namespace Modules.BuildingsModule.ViewsMediators
{
    public class PlacedBuildingsMediator : IMediator
    {
        [Inject]       private PlacedBuildingsView      _view            { get; set; }
        [InjectSignal] private BuildingsInternalSignals _internalSignals { get; set; }

        public void OnRegister() => _internalSignals.ShowBuilding.AddListener(OnShowBuilding);

        public void OnRemove() => _internalSignals.ShowBuilding.RemoveListener(OnShowBuilding);

        private void OnShowBuilding(PlacedBuildingVO building) =>
            _view.PlaceBuilding(building.Building, building.Type, building.Sprite, building.Area);
    }
}
