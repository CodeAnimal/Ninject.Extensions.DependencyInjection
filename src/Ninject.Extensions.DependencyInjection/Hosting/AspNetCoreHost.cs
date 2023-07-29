using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Ninject.Web.Common.SelfHost;


namespace Ninject.Extensions.DependencyInjection.Hosting
{
	public class AspNetCoreHost : INinjectSelfHost
	{
		private readonly AspNetCoreHostConfiguration configuration;
		private readonly Action<IKernel> configurationAction;

		public AspNetCoreHost(AspNetCoreHostConfiguration configuration, Action<IKernel> configurationAction = null)
		{
			this.configuration = configuration;
			this.configurationAction = configurationAction ?? (_ => { });
		}

		public void Start()
		{
			// The default web host builder takes care of
			// * Content and Web-root
			// * Loading appsettings.json files and environment variables configuration source
			// * Logging configuration (from appsettings.json)
			// * AllowedHosts configuration (from appsettings.json)
			var builder = configuration.WebHostBuilderFactory()
				.ConfigureServices(s => { s.AddNinject(configurationAction); });
			configuration.Apply(builder);

			var host = builder.Build();

			if (configuration == null || configuration.BlockOnStart)
			{
				host.Run();
			}
			else
			{
				host.RunAsync(configuration.CancellationToken);
			}
		}
	}
}
