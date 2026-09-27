using FlowIoC.BaseModule.Controller;
using FlowIoC.BaseModule.Injectable.Attributes;
using Modules.BuildingsModule.Models;

namespace Modules.BuildingsModule.Controllers
{
    /// <summary>The press ended: a grabbed placement stays where it is and stops following the pointer.</summary>
    internal class ReleasePlacementCommand : Command
    {
        [Inject] private IPlacementModel _placementModel { get; set; }

        public override void Execute() => _placementModel.Release();
    }
}
