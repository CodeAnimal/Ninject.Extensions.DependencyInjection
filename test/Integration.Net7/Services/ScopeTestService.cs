using System.Diagnostics.CodeAnalysis;
using Integration.Net7.Services.Abstractions;

namespace Integration.Net7.Services;

[SuppressMessage("Design", "CA1812", Justification = "Is used via Ninject DI.")]
internal class ScopeTestService : IScopeTestService
{
    private readonly IScopedService scopedService;

    public ScopeTestService(IScopedService scopedService)
    {
        this.scopedService = scopedService;
    }

    public string GetValue()
    {
        return scopedService.Value;
    }
}