using FlowIoC.BaseModule.Adapters;
using FlowIoC.BaseModule.Constructables;
using FlowIoC.BaseModule.Injectable.Attributes;
using Modules.ScreenModule.PopupScreenModule.Data.UnityObjects;
using Modules.ScreenModule.PopupScreenModule.Data.ValueObjects;
using Modules.ScreenModule.PopupScreenModule.RootsContexts;
using UnityEngine;

namespace Modules.ScreenModule.PopupScreenModule.Models
{
    /// <summary>
    /// Reads CD_PopupScreen off the Root's adapter in PostConstruct - the screen's context is listed on ScreenRoot, so
    /// that is the Root.
    /// </summary>
    internal class PopupScreenModel : IPopupScreenModel, IConstructable
    {
        [Inject(nameof(PopupScreenContext))]
        private GameObject _root { get; set; }

        public bool IsPostConstructed { get; set; }
        public bool IsDeconstructed { get; set; }

        public PopupAnimationVO Animation { get; private set; }

        public void PostConstruct()
        {
            var config = _root.GetComponent<RootAdapter>().GetScriptable<CD_PopupScreen>();
            Animation = new PopupAnimationVO(config.PopDuration, config.PopFromScale);
        }

        public void Deconstruct() { }
    }
}
