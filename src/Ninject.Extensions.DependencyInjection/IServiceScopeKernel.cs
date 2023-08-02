using Microsoft.Extensions.DependencyInjection;

namespace Ninject.Extensions.DependencyInjection
{
    public interface IServiceScopeKernel : IKernel
    {
        IServiceScope RootScope { get; }
    }
}