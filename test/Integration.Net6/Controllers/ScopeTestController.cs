using System;
using System.Threading.Tasks;
using Integration.Net6.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Integration.Net6.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ScopeTestController : ControllerBase
    {
        private readonly IServiceProvider serviceProvider;
        
        public ScopeTestController(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        [HttpGet]
        public async Task<ActionResult<bool>> Get()
        {
            var beforeScopedService = serviceProvider.GetRequiredService<IScopedService>();
            var beforeScopeTestService = serviceProvider.GetRequiredService<IScopeTestService>();
            
            await using var scope = serviceProvider.CreateAsyncScope();
            
            var scopedService = scope.ServiceProvider.GetRequiredService<IScopedService>();
            var scopeTestService = scope.ServiceProvider.GetRequiredService<IScopeTestService>();
            
            scopedService.Value = "After Value";
            beforeScopedService.Value = "Before Value";
            
            return scopeTestService.GetValue() == "After Value" 
                   && beforeScopeTestService.GetValue() == "Before Value";
        }
    }
}
