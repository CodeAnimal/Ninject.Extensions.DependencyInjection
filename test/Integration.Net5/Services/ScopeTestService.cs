using Integration.Net5.Services.Abstractions;

namespace Integration.Net5.Services
{
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
}