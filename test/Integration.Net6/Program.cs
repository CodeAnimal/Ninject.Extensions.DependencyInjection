using Integration.Net6;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Ninject.Extensions.DependencyInjection;

var builder = Host.CreateDefaultBuilder(args);

builder
    .UseServiceProviderFactory(new NinjectServiceProviderFactory())
    .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>());

var app = builder.Build();

await app.RunAsync().ConfigureAwait(false);