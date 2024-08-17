using FiszkIt.Api;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace FiszkIt.Application.Tests.Integration;

public class WebAppFactory : WebApplicationFactory<IWebAppMarker>, IAsyncLifetime
{
    public async Task InitializeAsync()
    {
    }

    public new async Task DisposeAsync()
    {
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);
    }

}