// Copyright (c) Autofac Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

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
        var client = AppFactory.CreateClient();


        var response = await client.GetAsync(new Uri("/Test", UriKind.Relative)).ConfigureAwait(false);


        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        Assert.Equal(10, content.Length);
    }

    [Fact]
    public async Task ScopeTest()
    {
        var client = AppFactory.CreateClient();


        var response = await client.GetAsync(new Uri("/ScopeTest", UriKind.Relative)).ConfigureAwait(false);


        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        Assert.Equal("true", content);
    }
}
