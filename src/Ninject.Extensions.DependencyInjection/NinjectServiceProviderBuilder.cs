using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace Ninject.Extensions.DependencyInjection
{
	public class NinjectServiceProviderBuilder
	{
		private readonly IServiceScopeKernel kernel;

		public NinjectServiceProviderBuilder(IServiceScopeKernel kernel)
		{
			this.kernel = kernel;
		}

		public IServiceProvider Build()
		{
			kernel.Bind<IServiceProvider>().ToMethod(context => {
				// Another interesting quirk of the Microsoft DI framework is that injecting the IServiceProvider injects a different
				// instance depending on the context. EXCEPT if the service provider is injected into a singleton in which case the
				// root service provider is used...
				//
				// It essentially always injects the instance that is being used for the instantiation
				// which kinda makes sense, but is not documented ANYWHERE, not tested ANYWHERE and was really quite difficult to
				// figure out.
				var scopeProvider = context.Parameters.OfType<ServiceProviderScopeParameter>().SingleOrDefault()?.SourceServiceProvider;
				if (scopeProvider != null)
				{
					var descriptor = context.Request.ParentContext?.Binding.Metadata.Get<ServiceDescriptor>(nameof(ServiceDescriptor));
					if (descriptor == null || descriptor.Lifetime != ServiceLifetime.Singleton)
					{
						return scopeProvider;
					}
				}

				return kernel.RootScope.ServiceProvider;
			});
#if NET6_0_OR_GREATER
			kernel.Bind<IServiceProviderIsService>().To<NinjectServiceProviderIsService>().InSingletonScope();
#endif

			return kernel.RootScope.ServiceProvider;
		}

	}
}
