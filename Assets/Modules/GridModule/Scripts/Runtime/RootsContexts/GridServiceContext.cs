using FlowIoC.BaseModule.Contexts;
using Modules.GridModule.Models;
using Modules.GridModule.Services;

namespace Modules.GridModule.RootsContexts
{
    public class GridServiceContext : Context
    {
        public override void InjectionBindings()
        {
            base.InjectionBindings();

            InjectionBinder.Bind<IGridModel, GridModel>();

            // The one type other modules reference directly, which is what makes this a Service.
            InjectionBinderCrossContext.Bind<IGridService, GridService>();
        }
    }
}
