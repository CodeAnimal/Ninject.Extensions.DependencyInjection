﻿using Microsoft.Extensions.DependencyInjection;
using Ninject.Modules;
using Ninject.Web.Common;
using System.Linq;
using Ninject.Extensions.DependencyInjection.Hosting;

namespace Ninject.Extensions.DependencyInjection
{
	/// <summary>
	/// Defines the bindings for the ASP.NET Core extension
	/// </summary>
	public class AspNetCoreModule : NinjectModule
	{
		public override void Load()
		{
            Kernel!.Components.Add<INinjectHttpApplicationPlugin, AspNetCoreApplicationPlugin>(); // provides the scope object for InRequestScope bindings
			Kernel.Bind<IServiceScopeFactory>().ToConstant((IServiceScopeFactory) Kernel);
			// FixServicesForPublicatonAdapter can only work when it is used through the AspNetCoreHostConfiguration mechanism which is not always used
			// like for example in the context of a Blazor application.
			Kernel.Bind<IPopulateAdapter>().To<ControllerActivationAdapter>().When(request => Kernel.GetBindings(typeof(AspNetCoreHostConfiguration)).Any());
		}
	}
}
