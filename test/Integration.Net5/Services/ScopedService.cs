using Integration.Net5.Services.Abstractions;

namespace Integration.Net5.Services
{
    internal class ScopedService : IScopedService
    {
        public string Value { get; set; }
    }
}