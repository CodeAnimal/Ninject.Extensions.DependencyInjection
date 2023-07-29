using System.Diagnostics.CodeAnalysis;
using Integration.Net6.Services;
using Integration.Net6.Services.Abstractions;
using Ninject;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Integration.Net6;

[SuppressMessage("CA1052", "CA1052", Justification = "Startup must not be a static class.")]
public class Startup
{
    public static void Configure(IApplicationBuilder app)
    {
        app.UseRouting()
           .UseEndpoints(endpoints => endpoints.MapControllers());
    }

    public static void ConfigureServices(IServiceCollection services)
    {
        services
            .AddTransient<IServiceA, ServiceA>()
            .AddTransient<IScopeTestService, ScopeTestService>()
            .AddScoped<IScopedService, ScopedService>();
        services.AddMvc();
    }

    public static void ConfigureContainer(IKernel builder)
    {
	    builder.Bind<IServiceB>().To<ServiceB>();
    }
}
