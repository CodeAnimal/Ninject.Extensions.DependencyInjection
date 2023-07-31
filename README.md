# Ninject.Extensions.DependencyInjection
[![GitHub](https://img.shields.io/github/license/mashape/apistatus.svg)](https://github.com/CodeAnimal/Ninject.Extensions.DependencyInjection/blob/main/LICENSE) [![CI](https://github.com/CodeAnimal/Ninject.Extensions.DependencyInjection/actions/workflows/ci.yml/badge.svg)](https://github.com/CodeAnimal/Ninject.Extensions.DependencyInjection/actions/workflows/ci.yml)

## Overview
**This repo is based on https://github.com/lord-executor/Ninject.Web.AspNetCore which has the original work.**

This project provides full [Ninject](https://github.com/ninject/Ninject) integration with .NET Core projects. This integrates .NET Core service collection with the Ninject kernel and uses Ninject's kernel as the service provider.

* [Ninject.Extensions.DependencyInjection](https://www.nuget.org/packages/Ninject.Extensions.DependencyInjection/)
* [Ninject.Extensions.DependencyInjection.AspNetCore](https://www.nuget.org/packages/Ninject.Extensions.DependencyInjection.AspNetCore/) - this is only required if you use `.InRequestScope()`.

## Examples

### Simple Program.cs
```csharp
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Ninject;
using Ninject.Extensions.DependencyInjection;

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
//     .AddTransient<IServiceB, ServiceB>();

builder.Services.AddMvc();

var app = builder.Build();

app.UseRouting()
    .UseEndpoints(endpoints => endpoints.MapControllers());

await app.RunAsync();
```

### With Startup.cs file
**Program.cs**
```csharp
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Ninject.Extensions.DependencyInjection;

var builder = Host.CreateDefaultBuilder(args);

builder
    .UseServiceProviderFactory(new NinjectServiceProviderFactory())
    .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>());

var app = builder.Build();

await app.RunAsync().ConfigureAwait(false);
```

**Startup.cs**
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
        //     .AddTransient<IServiceB, ServiceB>();
        
        services.AddMvc();
    }

    public static void ConfigureContainer(IKernel builder)
    {
        // Ninject Binding here
        // kernel.Bind<IServiceA>().To<ServiceA>();
    }
}
```