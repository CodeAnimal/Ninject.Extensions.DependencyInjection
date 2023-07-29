using Ninject.Modules;
using Ninject.Web.Common;

namespace Ninject.Extensions.DependencyInjection.AspNetCore
{
    public class AspNetCoreModule : NinjectModule
    {
        public override void Load()
        {
            Kernel!.Components.Add<INinjectHttpApplicationPlugin, AspNetCoreApplicationPlugin>(); // provides the scope object for InRequestScope bindings
        }
    }
}

