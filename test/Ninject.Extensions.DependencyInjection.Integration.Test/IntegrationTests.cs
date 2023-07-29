using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

#if NET7_0
using Integration.Net7;
#endif
#if NET6_0
using Integration.Net6;
#endif
#if NET5_0
using Integration.Net5;
#endif

namespace Ninject.Extensions.DependencyInjection.Integration.Test;

#if NET7_0
    public class IntegrationTests : IClassFixture<WebApplicationFactory<Program>>
#else
    public class IntegrationTests : IClassFixture<WebApplicationFactory<Startup>>
#endif
    {
#if NET7_0
    public IntegrationTests(WebApplicationFactory<Program> appFactory)
#else
    public IntegrationTests(WebApplicationFactory<Startup> appFactory)
#endif
    {
        AppFactory = appFactory;
    }

#if NET7_0
    public WebApplicationFactory<Program> AppFactory { get; }
#else
    public WebApplicationFactory<Startup> AppFactory { get; }
#endif

    [Fact]
    public async Task GetString()
    {
        // Arrange
        var client = AppFactory.CreateClient();

        // Act
        var response = await client.GetAsync(new Uri("/Test", UriKind.Relative)).ConfigureAwait(false);

        // Assert
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        Assert.Equal(10, content.Length);
    }

    [Fact]
    public async Task ScopeTest()
    {
        // Arrange
        var client = AppFactory.CreateClient();

        // Act
        var response = await client.GetAsync(new Uri("/ScopeTest", UriKind.Relative)).ConfigureAwait(false);

        // Assert
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        Assert.Equal("true", content);
    }
}
