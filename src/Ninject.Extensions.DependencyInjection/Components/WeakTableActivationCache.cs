﻿using Ninject.Activation.Caching;
using Ninject.Components;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Ninject.Extensions.DependencyInjection.Components
{
	/// <summary>
	/// Stores the objects that were activated / deactivated in a weak table.
	/// </summary>
	public class WeakTableActivationCache : NinjectComponent, IActivationCache, IPruneable, IActivationCacheAccessor
	{
		/// <summary>
		/// Using the https://docs.microsoft.com/en-us/dotnet/api/system.runtime.compilerservices.conditionalweaktable-2?view=net-5.0 to simplify the
		/// implementation and also provide an additional API to simplify the implementation of ordered disposal.
		/// 
		/// It also eliminates the need for explicit locking since the conditional weak table is thread safe already.
		/// </summary>
		private readonly ConditionalWeakTable<object, ActivationEntry> trackedInstances = new ConditionalWeakTable<object, ActivationEntry>();

		public WeakTableActivationCache()
		{
		}

		/// <summary>
		/// Gets the activated object count.
		/// </summary>
		/// <value>The activated object count.</value>
		public int ActivatedObjectCount
		{
			get
			{
				return trackedInstances.Count(kvp => !kvp.Value.IsDeactivated);
			}
		}

		/// <summary>
		/// Gets the deactivated object count.
		/// </summary>
		/// <value>The deactivated object count.</value>
		public int DeactivatedObjectCount
		{
			get
			{
				return trackedInstances.Count(kvp => kvp.Value.IsDeactivated);
			}
		}

		/// <summary>
		/// Clears the cache.
		/// </summary>
		public void Clear()
		{
			trackedInstances.Clear();
		}

		/// <summary>
		/// Adds an activated instance.
		/// </summary>
		/// <param name="instance">The instance to be added.</param>
		public void AddActivatedInstance(object instance)
		{
			// we don't actually care about the value that may already be in there, but we only
			// want to create a new one if there isn't one already
			trackedInstances.GetValue(instance, inst => new ActivationEntry(inst));
		}

		/// <summary>
		/// Adds an deactivated instance.
		/// </summary>
		/// <param name="instance">The instance to be added.</param>
		public void AddDeactivatedInstance(object instance)
		{
			// if the instance has been activated before, then we can just set that entry to deactivated,
			// otherwise a new activation entry is added - the scenario of deactivating instances that
			// were not activated before is (probably) only really relevant for tests anyway.
			trackedInstances.GetValue(instance, inst => new ActivationEntry(inst)).IsDeactivated = true;
		}

		/// <summary>
		/// Determines whether the specified instance is activated.
		/// </summary>
		/// <param name="instance">The instance.</param>
		/// <returns>
		///     <c>true</c> if the specified instance is activated; otherwise, <c>false</c>.
		/// </returns>
		public bool IsActivated(object instance)
		{
			return trackedInstances.TryGetValue(instance, out var entry) && !entry.IsDeactivated;
		}

		/// <summary>
		/// Determines whether the specified instance is deactivated.
		/// </summary>
		/// <param name="instance">The instance.</param>
		/// <returns>
		///     <c>true</c> if the specified instance is deactivated; otherwise, <c>false</c>.
		/// </returns>
		public bool IsDeactivated(object instance)
		{
			return trackedInstances.TryGetValue(instance, out var entry) && entry.IsDeactivated;
		}

		/// <summary>
		/// Prunes this instance.
		/// </summary>
		public void Prune()
		{
			// not actually needed with the ConditionalWeakTable since that effectively prunes itself
		}

		public IActivationEntry GetEntry(object instance)
		{
			return trackedInstances.TryGetValue(instance, out var entry) ? entry : null;
		}

		public IEnumerable<IActivationEntry> GetAllEntries()
		{
			return trackedInstances.Select(kvp => kvp.Value);
		}
	}
}