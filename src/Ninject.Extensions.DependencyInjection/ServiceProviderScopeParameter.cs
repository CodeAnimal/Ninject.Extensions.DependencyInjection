using Ninject.Infrastructure.Disposal;
using Ninject.Parameters;
using System;
using System.Collections.Generic;

namespace Ninject.Extensions.DependencyInjection
{
	public class ServiceProviderScopeParameter : Parameter
	{
		private readonly NinjectServiceScope scope;
		private readonly IList<TransientScope> children = new List<TransientScope>();

		public IServiceProvider SourceServiceProvider => scope.ServiceProvider;

		public ServiceProviderScopeParameter(NinjectServiceScope scope)
			: base(nameof(ServiceProviderScopeParameter), scope, true)
		{
			this.scope = scope;
			this.scope.Disposed += (_, _) =>
			{
				foreach (var child in children)
				{
					child.Dispose();
				}
			};
		}

		public DisposableObject DeriveTransientScope()
		{
			var child = new TransientScope();
			children.Add(child);
			return child;
		}

		private class TransientScope : DisposableObject
		{
		}
	}
}
