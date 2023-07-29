using Ninject.Activation;
using Ninject.Parameters;
using Ninject.Planning.Bindings;
using Ninject.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ninject.Extensions.DependencyInjection
{
	public class ServiceProviderScopeResolutionRoot : IResolutionRoot
	{
		private readonly IResolutionRoot parent;
		private readonly ServiceProviderScopeParameter scopeParameter;

		public ServiceProviderScopeResolutionRoot(IResolutionRoot parent, NinjectServiceScope scope)
		{
			this.parent = parent;
			scopeParameter = new ServiceProviderScopeParameter(scope);
		}

		public bool CanResolve(IRequest request)
		{
			return parent.CanResolve(request);
		}

		public bool CanResolve(IRequest request, bool ignoreImplicitBindings)
		{
			return parent.CanResolve(request, ignoreImplicitBindings);
		}

		public IRequest CreateRequest(Type service, Func<IBindingMetadata, bool> constraint, IEnumerable<IParameter> parameters, bool isOptional, bool isUnique)
		{
			var updatedParameters = parameters.ToList() ?? new List<IParameter>();
			updatedParameters.Add(scopeParameter);
			return parent.CreateRequest(service, constraint, updatedParameters, isOptional, isUnique);
		}

		public void Inject(object instance, params IParameter[] parameters)
		{
			parent.Inject(instance, parameters);
		}

		public bool Release(object instance)
		{
			return parent.Release(instance);
		}

		public IEnumerable<object> Resolve(IRequest request)
		{
			return parent.Resolve(request);
		}
	}
}
