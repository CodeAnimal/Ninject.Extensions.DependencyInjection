using Ninject.Infrastructure;
using System.Collections.Generic;

namespace Ninject.Extensions.DependencyInjection.Components
{
	public interface IActivationCacheAccessor
	{
		IActivationEntry GetEntry(object instance);
		IEnumerable<IActivationEntry> GetAllEntries();
	}

	public interface IActivationEntry
	{
		ReferenceEqualWeakReference Reference { get; }
		long Order { get; }
	}
}
