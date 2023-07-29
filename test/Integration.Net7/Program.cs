using System.Threading.Tasks;
using Ninject.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Integration.Net7;

public static class Program
{
    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .UseServiceProviderFactory(new NinjectServiceProviderFactory())
            .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>());

    public static async Task Main(string[] args) => await CreateHostBuilder(args).Build().RunAsync().ConfigureAwait(false);
}
