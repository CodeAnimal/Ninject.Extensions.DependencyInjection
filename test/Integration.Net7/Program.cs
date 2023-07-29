using Integration.Net7.Services;
using Integration.Net7.Services.Abstractions;
using Microsoft.AspNetCore.Builder;
using Ninject.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Ninject;

var builder = WebApplication.CreateBuilder(args);

builder.Host
    .UseServiceProviderFactory(new NinjectServiceProviderFactory())
    .ConfigureContainer<IKernel>(kernel =>
    {
        kernel.Bind<IServiceB>().To<ServiceB>();
    });

builder.Services
    .AddTransient<IServiceA, ServiceA>()
    .AddTransient<IScopeTestService, ScopeTestService>()
    .AddScoped<IScopedService, ScopedService>();
        
builder.Services.AddMvc();


var app = builder.Build();

app.UseRouting()
    .UseEndpoints(endpoints => endpoints.MapControllers());

await app.RunAsync().ConfigureAwait(false);

public abstract partial class Program {}