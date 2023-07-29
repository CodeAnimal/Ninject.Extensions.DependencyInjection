using System.Diagnostics.CodeAnalysis;
using Integration.Net6.Services.Abstractions;

namespace Integration.Net6.Services;

[SuppressMessage("Design", "CA1812", Justification = "Is used via Ninject DI.")]
internal class ScopedService : IScopedService
{
    public string Value { get; set; } = string.Empty;
}