using System;
using Microsoft.Extensions.DependencyInjection;

namespace Ninject.Extensions.DependencyInjection
{
	/// <summary>
	/// Extension methods on <see cref="IServiceCollection"/> to register the <see cref="IServiceProviderFactory{TContainerBuilder}"/>.
	/// </summary>
	public static class ServiceCollectionExtensions
	{
		/// <summary>
		/// Adds the <see cref="NinjectServiceProviderFactory"/> to the service collection. 
		/// </summary>
		/// <param name="services">The service collection to add the factory to.</param>
		/// <param name="configurationAction">Action on a <see cref="IKernel"/> that adds component registrations to the container.</param>
		/// <returns>The service collection.</returns>
		public static IServiceCollection AddNinject(this IServiceCollection services, Action<IKernel> configurationAction = null)
		{
			return services.AddSingleton<IServiceProviderFactory<IKernel>>(new NinjectServiceProviderFactory(configurationAction));
		}
	}
}
