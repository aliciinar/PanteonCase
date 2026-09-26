using FlowIoC.BaseModule.Contexts;
using Modules.GridModule.Models;
using Modules.GridModule.Services;

namespace Modules.GridModule.RootsContexts
{

    public class GridServiceContext : Context
    {
        public override void SignalBindings()
        {
            base.SignalBindings();
        }

        public override void InjectionBindings()
        {
            base.InjectionBindings();

            InjectionBinder.Bind<IGridModel, GridModel>();

            // The one type other modules reference directly, which is what makes this a Service.
            InjectionBinderCrossContext.Bind<IGridService, GridService>();
        }

        public override void MediationBindings()
        {
            base.MediationBindings();
        }

        public override void CommandBindings()
        {
            base.CommandBindings();
        }

        public override void Setup()
        {
            base.Setup();
        }

        public override void Launch()
        {
            base.Launch();
        }
    }
}
