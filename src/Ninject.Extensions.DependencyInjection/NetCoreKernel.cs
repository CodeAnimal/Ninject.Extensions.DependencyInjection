using Microsoft.Extensions.DependencyInjection;
using Ninject.Activation;
using Ninject.Activation.Caching;
using Ninject.Activation.Strategies;
using Ninject.Modules;
using Ninject.Planning.Bindings;
using Ninject.Planning.Bindings.Resolvers;
using System;
using Ninject.Extensions.DependencyInjection.Components;

namespace Ninject.Extensions.DependencyInjection
{
	public class NetCoreKernel : StandardKernel, IServiceScopeFactory
	{
		public IServiceScope RootScope { get; }

		public NetCoreKernel(params INinjectModule[] modules)
			: base(modules)
		{
			RootScope = new NinjectServiceScope(this, true);
			Settings.AllowNullInjection = true;
		}

		public NetCoreKernel(INinjectSettings settings, params INinjectModule[] modules)
			: base(settings ?? new NinjectSettings(), modules)
		{
			RootScope = new NinjectServiceScope(this, true);
			Settings.AllowNullInjection = true;
		}

		protected override Func<IBinding, bool> SatifiesRequest(IRequest request)
		{
			return binding => {
				var latest = true;
				if (request.IsUnique && request.Constraint == null)
				{
					latest = binding.Metadata.Get<BindingIndex.Item>(nameof(BindingIndex))?.IsLatest ?? true;
				}
				return binding.Matches(request) && request.Matches(binding) && latest;
			};
		}

		protected override void AddComponents()
		{
			base.AddComponents();
			Components.RemoveAll<IActivationCache>();
			Components.Add<IActivationCache, WeakTableActivationCache>();
			Components.Remove<IBindingResolver, OpenGenericBindingResolver>();
			Components.Add<IBindingResolver, ConstrainedGenericBindingResolver>();
			Components.Remove<IBindingPrecedenceComparer, BindingPrecedenceComparer>();
			Components.Add<IBindingPrecedenceComparer, IndexedBindingPrecedenceComparer>();

			Components.Add<IDisposalManager, DisposalManager>();
			Components.Remove<IActivationStrategy, DisposableStrategy>();
			Components.Add<IActivationStrategy, OrderedDisposalStrategy>();
		}

		public void DisableAutomaticSelfBinding()
		{
			Components.Remove<IMissingBindingResolver, SelfBindingResolver>();
		}

		public override void Dispose(bool disposing)
		{
			if (disposing && !IsDisposed)
			{
				RootScope.Dispose();
			}

			base.Dispose(disposing);
		}

		public IServiceScope CreateScope()
		{
			return new NinjectServiceScope(this, false);
		}
	}
}
