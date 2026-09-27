#if UNITY_EDITOR
using Modules.GameplayModule.InfoScreenModule.ViewsMediators;
using FlowIoC.BaseModule.Attributes;
using FlowIoC.ScreenModule.RootsContexts;

namespace Modules.GameplayModule.InfoScreenModule.InfoScreenTestModule.RootsContexts
{
    /// <summary>
    /// A screen's test context opens the screen and nothing else: the production context, listed
    /// as a sub-context on the test Root, brings the signals, the mediation and the ScreenCVO.
    /// </summary>
    [ExcludeFromContextWindow]
    public class InfoScreenTestContext : BaseScreenContext
    {
        public override void Launch()
        {
            base.Launch();
			_screenService.Open<InfoScreenView>().Show();

        }
    }
}
#endif
