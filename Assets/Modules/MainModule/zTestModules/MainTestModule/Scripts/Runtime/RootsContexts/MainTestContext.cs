#if UNITY_EDITOR
using FlowIoC.BaseModule.Contexts;
using FlowIoC.BaseModule.SharedData;
using FlowIoC.ConsoleModule;
using Modules.BuildingsModule.Shared.Data.UnityObjects;
using Modules.UnitsModule.Shared.Data.UnityObjects;
using UnityEngine;

namespace Modules.MainModule.MainTestModule.RootsContexts
{
    /// <summary>
    /// The design test scene: the whole game, booted by MainRoot as in MainScene, on the test copies of the design
    /// configs in MainTestModule/Scriptables - assigned in this scene to the RootAdapters of the Roots that read them.
    /// This context checks that the shared ones are the copies and says so once.
    /// </summary>
    public class MainTestContext : Context
    {
        private const string TestSuffix = "_Test";

        public override void Setup()
        {
            base.Setup();

            var sharedData = InjectionBinderCrossContext.GetInstance<ISharedDataModel>();
            Check(sharedData.GetScriptable<CD_Buildings>());
            Check(sharedData.GetScriptable<CD_Units>());

            FlowLogger.Log("Design test scene - the game runs on the *_Test configs in MainTestModule/Scriptables.");
        }

        private static void Check(ScriptableObject config)
        {
            if (!config.name.EndsWith(TestSuffix))
                FlowLogger.LogError($"Design test scene - {config.name} is the shipped config, not a test copy. " +
                                    "Assign its _Test copy on the RootAdapter of the Root that files it, in this scene.");
        }
    }
}
#endif
