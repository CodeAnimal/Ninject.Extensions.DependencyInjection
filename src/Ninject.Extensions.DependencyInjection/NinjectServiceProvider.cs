﻿using Microsoft.Extensions.DependencyInjection;
using Ninject.Syntax;
using System;

namespace Ninject.Extensions.DependencyInjection
{
	/// <summary>
	/// We wrap the <see cref="IResolutionRoot" /> here to explicitly implement both the <see cref="IServiceProvider" /> and
	/// <see cref="ISupportRequiredService" /> to give us more control.
	/// 
	/// Note: ASP.NET Core wants to use a method from ISupportRequiredService to resolve a non-optional service.
	/// Although it's implemented on Microsoft.Extensions.DependencyInjection.ServiceProviderServiceExtensions in a generic way
	/// we implement it here to have the nicer exceptions from Ninject so that it's possible to distinguish the "not registered at all"
	/// vs the "ambigious matches found" cases.
	/// 
	/// Also, even though <see cref="IServiceProvider"/> does NOT implement <see cref="IDisposable"/>,
	/// Microsoft.Extensions.DependencyInjection assumes that the service provider does implement it and disposes all of its
	/// instances that are associated with the root scope. This is why we implement <see cref="IDisposable"/> and this is why
	/// we pass an <see cref="IServiceScope"/> constructor argument when creating the root service provider.
	/// </summary>
	public class NinjectServiceProvider : IServiceProvider, ISupportRequiredService, IDisposable
	{
		private readonly IResolutionRoot resolutionRoot;
		private readonly IServiceScope scope;

		public NinjectServiceProvider(IResolutionRoot resolutionRoot, IServiceScope scope)
		{
			this.resolutionRoot = resolutionRoot;
			this.scope = scope;
		}

		public object GetRequiredService(Type serviceType)
		{
			var result = resolutionRoot.Get(serviceType);
			return result;
		}

		public object GetService(Type serviceType)
		{
			var result = resolutionRoot.TryGet(serviceType);
			return result;
		}

        public void Dispose()
        {
			scope?.Dispose();
            GC.SuppressFinalize(this);
		}
	}
}
