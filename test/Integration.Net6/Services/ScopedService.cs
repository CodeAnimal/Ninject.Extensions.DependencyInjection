using Integration.Net6.Services.Abstractions;

namespace Integration.Net6.Services;

internal class ScopedService : IScopedService
{
    public string Value { get; set; }
}