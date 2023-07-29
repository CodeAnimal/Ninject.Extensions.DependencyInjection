﻿using Microsoft.AspNetCore.Hosting;
using System;
using System.Threading;

namespace Ninject.Extensions.DependencyInjection.Hosting
{
	public interface IAspNetCoreHostConfiguration
	{
		void ConfigureWebHostBuilder(Func<IWebHostBuilder> webHostBuilderFactory);

		void ConfigureStartupType(Type startupType);

		void ConfigureHostingModel(Action<IWebHostBuilder> configureAction);

		void ConfigureStartupBehavior(bool blockOnStart, CancellationToken cancellationToken);

		void ConfigureCustomControllerActivator(Type controllerActivatorType);
	}
}
