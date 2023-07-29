using System.Diagnostics.CodeAnalysis;
using Integration.Net7.Services.Abstractions;

namespace Integration.Net7.Services;

[SuppressMessage("Design", "CA1812", Justification = "Is used via Ninject DI.")]
internal class ServiceB : IServiceB
{
    private readonly IServiceA serviceA;

    public ServiceB(IServiceA serviceA)
    {
        this.serviceA = serviceA;
    }

    public string GetFromSubService(int size)
    {
        return serviceA.GetRandomString(size);
    }
}