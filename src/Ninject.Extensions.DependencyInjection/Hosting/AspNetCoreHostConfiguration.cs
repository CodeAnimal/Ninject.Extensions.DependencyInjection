using Microsoft.AspNetCore.Hosting;
using System;
using System.Threading;

namespace Ninject.Extensions.DependencyInjection.Hosting
{
	public class AspNetCoreHostConfiguration : IAspNetCoreHostConfiguration
	{
		private Type customStartup;
		private Action<IWebHostBuilder> hostingModelConfigurationAction;

		internal Func<IWebHostBuilder> WebHostBuilderFactory { get; private set; }
		internal bool BlockOnStart { get; private set; }
		internal CancellationToken CancellationToken { get; private set; }
		internal Type CustomControllerActivator { get; private set; }

		public AspNetCoreHostConfiguration(string[] cliArgs = null)
		{
			customStartup = typeof(EmptyStartup);
			WebHostBuilderFactory = () => new DefaultWebHostConfiguration(cliArgs).ConfigureAll().GetBuilder();
		}

		void IAspNetCoreHostConfiguration.ConfigureWebHostBuilder(Func<IWebHostBuilder> webHostBuilderFactory)
		{
			WebHostBuilderFactory = webHostBuilderFactory;
		}

		void IAspNetCoreHostConfiguration.ConfigureStartupType(Type startupType)
		{
			if (!typeof(AspNetCoreStartupBase).IsAssignableFrom(startupType))
			{
				throw new ArgumentException("Startup type must inherit from " + nameof(AspNetCoreStartupBase));
			}
			customStartup = startupType;
		}

		void IAspNetCoreHostConfiguration.ConfigureHostingModel(Action<IWebHostBuilder> configureAction)
		{
			hostingModelConfigurationAction = configureAction;
		}

		void IAspNetCoreHostConfiguration.ConfigureStartupBehavior(bool blockOnStart, CancellationToken cancellationToken)
		{
			BlockOnStart = blockOnStart;
			CancellationToken = cancellationToken;
		}

		internal virtual void Apply(IWebHostBuilder builder)
		{
			hostingModelConfigurationAction?.Invoke(builder);
			builder.UseStartup(customStartup);
		}

		void IAspNetCoreHostConfiguration.ConfigureCustomControllerActivator(Type controllerActivatorType)
		{
			CustomControllerActivator = controllerActivatorType;
		}
	}
}
