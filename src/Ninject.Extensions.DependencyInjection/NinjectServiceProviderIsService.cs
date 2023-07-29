using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace Ninject.Extensions.DependencyInjection
{
#if NET6_0_OR_GREATER
	public class NinjectServiceProviderIsService : IServiceProviderIsService
	{
		private readonly IKernel kernel;

		public NinjectServiceProviderIsService(IKernel kernel)
		{
			this.kernel = kernel;
		}

		public bool IsService(Type serviceType)
		{
			// IsService should only return true if the type can actually be resolved to a service
			// and open generic types cannot. Except for IEnumerable<T> which should return true
			// in ANY case (see DependencyInjectionSpecificationTests.IEnumerableWithIsServiceAlwaysReturnsTrue)
			if (serviceType.IsGenericTypeDefinition)
			{
				return false;
			}

			if (serviceType.IsGenericType && serviceType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
			{
				return true;
			}

			return kernel.CanResolve(serviceType);
		}
	}
#endif
}
