using System;
using System.Collections.Generic;

namespace Ninject.Extensions.DependencyInjection
{
	public class BindingIndex
	{
		private readonly IDictionary<Type, Item> bindingIndexMap = new Dictionary<Type, Item>();

		public int Count { get; private set; }

		public BindingIndex()
		{
		}

		public Item Next(Type serviceType)
		{
			
			bindingIndexMap.TryGetValue(serviceType, out var previous);

			var next = new Item(this, serviceType, Count++, previous?.TypeIndex + 1 ?? 0);
			bindingIndexMap[serviceType] = next;

			return next;
		}

		private bool IsLatest(Type serviceType, Item item)
		{
			return bindingIndexMap[serviceType] == item;
		}

		public class Item
		{
			private readonly BindingIndex root;
			private readonly Type serviceType;

			public int TotalIndex { get; }
			public int TypeIndex { get; }

			public bool IsLatest => root.IsLatest(serviceType, this);
			public int Precedence => root.Count - TotalIndex;

			public Item(BindingIndex root, Type serviceType, int totalIndex, int typeIndex)
			{
				this.root = root;
				this.serviceType = serviceType;
				TotalIndex = totalIndex;
				TypeIndex = typeIndex;
			}
		}
	}
}
