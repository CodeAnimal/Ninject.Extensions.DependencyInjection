using Ninject.Infrastructure.Disposal;
using System;
using System.Threading;

namespace Ninject.Extensions.DependencyInjection
{
	public class RequestScope : DisposableObject
	{
		private static readonly AsyncLocal<RequestScope> current = new();
		public static RequestScope Current => current.Value;

		public RequestScope()
		{
			if (current.Value == null)
			{
				current.Value = this;
			}
			else
			{
				throw new InvalidOperationException("Nesting of RequestScope is not allowed.");
			}
		}

		public override void Dispose(bool disposing)
		{
			if (disposing && !IsDisposed)
			{
				current.Value = null;
			}

			base.Dispose(disposing);
		}
	}
}
