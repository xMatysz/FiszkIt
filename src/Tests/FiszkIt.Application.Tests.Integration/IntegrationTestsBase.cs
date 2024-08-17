using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace FiszkIt.Application.Tests.Integration;

public class IntegrationTestsBase : IClassFixture<WebAppFactory>
{
    private readonly WebAppFactory _factory;
    protected IServiceProvider Services { get; set; }

    protected IntegrationTestsBase(WebAppFactory factory)
    {
        _factory = factory;

        StartNewScope();
    }

    private void StartNewScope()
    {
        var scope = _factory.Services.CreateScope();

        Services = scope.ServiceProvider;
    }
}