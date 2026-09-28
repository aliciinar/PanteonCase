#if UNITY_EDITOR
using FlowIoC.BaseModule.Attributes;
using FlowIoC.ScreenModule.RootsContexts;
using Modules.ScreenModule.PopupScreenModule.Signals;

namespace Modules.ScreenModule.PopupScreenModule.PopupScreenTestModule.RootsContexts
{
    /// <summary>
    /// A screen's test context opens the screen and nothing else: the production context, listed
    /// as a sub-context on the test Root, brings the signals, the mediation and the ScreenCVO. The popup
    /// opens the way any module opens it - a message on its incoming signal - and reads CD_PopupScreen
    /// off the test Root's adapter, where the scene files it.
    /// </summary>
    [ExcludeFromContextWindow]
    public class PopupScreenTestContext : BaseScreenContext
    {
        public override void Launch()
        {
            base.Launch();
            InjectionBinderCrossContext.GetInstance<PopupScreenSignals>().Incoming.ShowPopup
                                       .Dispatch("There is no free cell next to the target to attack from.");
        }
    }
}
#endif
