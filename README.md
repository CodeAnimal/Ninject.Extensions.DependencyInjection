# This repo is based on https://github.com/lord-executor/Ninject.Web.AspNetCore which has the original work.

[![GitHub](https://img.shields.io/github/license/mashape/apistatus.svg)](https://github.com/CodeAnimal/Ninject.Extensions.DependencyInjection/blob/main/LICENSE) [![CI](https://github.com/CodeAnimal/Ninject.Extensions.DependencyInjection/actions/workflows/ci.yml/badge.svg)](https://github.com/CodeAnimal/Ninject.Extensions.DependencyInjection/actions/workflows/ci.yml)


# Overview
This project provides full [Ninject](https://github.com/ninject/Ninject) integration with ASP.NET Core projects. Full integration means that the Ninject kernel is used to replace the standard service provider that comes with ASP.NET Core.

* Ninject.Extensions.DependencyInject
* Ninject.Extensions.DependencyInjection.AspNetCore - this is only required if you use `.InRequestScope()`.

# Examples

## Simple Program.cs
```csharp
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
        // Ninject Binding here
        // kernel.Bind<IServiceA>().To<ServiceA>();
    });

// ServiceCollection Binding here
// builder.Services
//    .AddTransient<IServiceB, ServiceB>();

builder.Services.AddMvc();

var app = builder.Build();

app.UseRouting()
    .UseEndpoints(endpoints => endpoints.MapControllers());

await app.RunAsync().ConfigureAwait(false);
```

## With Startup.cs file
```csharp
public class Startup
{
    public static void Configure(IApplicationBuilder app)
    {
        app.UseRouting()
           .UseEndpoints(endpoints => endpoints.MapControllers());
    }

    public static void ConfigureServices(IServiceCollection services)
    {
        // ServiceCollection Binding here
        // builder.Services
        //    .AddTransient<IServiceB, ServiceB>();
        
        services.AddMvc();
    }

    public static void ConfigureContainer(IKernel builder)
    {
	    // Ninject Binding here
        // kernel.Bind<IServiceA>().To<ServiceA>();
    }
}
```