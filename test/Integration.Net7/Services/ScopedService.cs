using Integration.Net7.Services.Abstractions;

namespace Integration.Net7.Services;

internal class ScopedService : IScopedService
{
    public string Value { get; set; }
}