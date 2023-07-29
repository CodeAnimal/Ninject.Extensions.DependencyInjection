# This repo is based on https://github.com/lord-executor/Ninject.Web.AspNetCore which has the original work.

[![GitHub](https://img.shields.io/github/license/mashape/apistatus.svg)](https://github.com/CodeAnimal/Ninject.Extensions.DependencyInjection/blob/main/LICENSE) [![CI](https://github.com/CodeAnimal/Ninject.Extensions.DependencyInjection/actions/workflows/ci.yml/badge.svg)](https://github.com/CodeAnimal/Ninject.Extensions.DependencyInjection/actions/workflows/ci.yml)


# Overview
This project provides full [Ninject](https://github.com/ninject/Ninject) integration with ASP.NET Core projects. Full integration means that the Ninject kernel is used to replace the standard service provider that comes with ASP.NET Core.

* Ninject.Extensions.DependencyInject
* Ninject.Extensions.DependencyInjection.AspNetCore - this only required if you use `.InRequestScope()`.

# Configuration
```cs
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