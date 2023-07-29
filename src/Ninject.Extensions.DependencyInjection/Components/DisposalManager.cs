using Ninject.Activation;
using Ninject.Activation.Caching;
using Ninject.Components;
using Ninject.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Ninject.Extensions.DependencyInjection.Components
{
	/// <summary>
	/// The disposal manager helps to dispose service instances in a deterministic order when a <see cref="NinjectServiceScope"/> is
	/// disposed. Specifically, it disposes services in the reverse order in which they were instantiated because that what
	/// is expected by a compliant implementation for Microsoft.Extensions.DependencyInjection. For more details, see
	/// DependencyInjectionComplianceTests with the tests:
	/// * DisposesInReverseOrderOfCreation
	/// * DisposingScopeDisposesService
	/// 
	/// This is accomplished with a customized implementation of the <see cref="IActivationCache"/> that also exposes some
	/// additional APIs in the form of <see cref="IActivationCacheAccessor"/>.
	/// </summary>
	public class DisposalManager : NinjectComponent, IDisposalManager
	{
		private readonly IActivationCacheAccessor cacheAccessor;
		private readonly AsyncLocal<IDisposalCollectorArea> area = new AsyncLocal<IDisposalCollectorArea>();

		public DisposalManager(IActivationCache activationCache)
		{
			if (activationCache == null)
			{
				throw new ArgumentNullException(nameof(activationCache));
			}

			if (activationCache is not IActivationCacheAccessor activationCacheAccessor)
			{
                throw new ArgumentException("DisposalManager expects the activation cache to also implement IActivationCacheAccessor", nameof(activationCache));
			}

			cacheAccessor = activationCacheAccessor;
		}

		public void RemoveInstance(InstanceReference instanceReference)
		{
			(area.Value as IDisposalCollector ?? ImmediateDisposal.Instance).Register(cacheAccessor.GetEntry(instanceReference.Instance));
		}

		/// <summary>
		/// This is meant as a debugging tool to allow an application to analyze which services are remaining
		/// active that maybe should not be. In particular in high-load scenarios where many services are created
		/// for each request, this can be useful to analze scoping issues.
		/// </summary>
		/// <returns>The list of all currently tracked references - note, that any weak reference can become "dead"
		/// at any point in time.</returns>
		public IList<ReferenceEqualWeakReference> GetActiveReferences()
		{
			return cacheAccessor.GetAllEntries().Select(entry => entry.Reference).ToList();
		}

		public IDisposalCollectorArea CreateArea()
		{
			if (area.Value == null)
			{
				return area.Value = new OrderedAggregateDisposalArea(this);
			}

			return new InnerDisposalArea(area.Value);
		}

		#region IDisposalCollectorArea Implementations

		/// <summary>
		/// This collector area essentially just "buffers" the disposal until the area itself is disposed at which point
		/// it goes through the active service instances in reverse and disposed all the services that are "marked"
		/// for disposal.
		/// </summary>
		private class OrderedAggregateDisposalArea : IDisposalCollectorArea
		{
			private readonly DisposalManager manager;
			private IList<IActivationEntry> disposals = new List<IActivationEntry>();

			public OrderedAggregateDisposalArea(DisposalManager manager)
			{
				this.manager = manager;
			}

			public void Dispose()
{
				if (disposals == null || disposals.Count == 0)
				{
					return;
				}

				foreach (var candidate in disposals.OrderByDescending(entry => entry.Order))
				{
					(candidate.Reference.Target as IDisposable)?.Dispose();
				}

				manager.area.Value = null;
				disposals = null;
			}

			public void Register(IActivationEntry activationEntry)
			{
				if (activationEntry != null)
				{
					disposals.Add(activationEntry);
				}
			}
		}

		/// <summary>
		/// If there is no active disposal collection area, we just immediately dispose as Ninject does normally.
		/// </summary>
		private class ImmediateDisposal : IDisposalCollector
		{
			public static ImmediateDisposal Instance { get; } = new ImmediateDisposal();

			public void Register(IActivationEntry activationEntry)
			{
				(activationEntry.Reference.Target as IDisposable)?.Dispose();
			}
		}

		/// <summary>
		/// When disposal collection areas are nested, everything is delegated to the outermost area
		/// and services are only disposed with the disposal of the outermost <see cref="OrderedAggregateDisposalArea"/>
		/// because only there do we know all the services that need to be disposed.
		/// </summary>
		private class InnerDisposalArea : IDisposalCollectorArea
		{
			private readonly IDisposalCollectorArea parent;

			public InnerDisposalArea(IDisposalCollectorArea parent)
			{
				this.parent = parent;
			}

			public void Dispose()
			{
				// nothing
			}

			public void Register(IActivationEntry activationEntry)
			{
				parent.Register(activationEntry);
			}
		}

		#endregion
	}
}
