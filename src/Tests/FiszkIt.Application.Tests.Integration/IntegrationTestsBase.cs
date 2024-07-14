using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace FiszkIt.Application.Tests.Integration;

public class IntegrationTestsBase : IClassFixture<WebAppFactory>, IAsyncLifetime
{
    private readonly WebAppFactory _factory;
    private FiszkItDbContext _dbContext;

    protected IServiceProvider Services { get; set; }

    protected IntegrationTestsBase(WebAppFactory factory)
    {
        _factory = factory;

        StartNewScope();
    }

    public Task InitializeAsync() => Task.CompletedTask;
    public async Task DisposeAsync() => await _factory.ResetDbAsync();

    private void StartNewScope()
    {
        var scope = _factory.Services.CreateScope();

        _dbContext = scope.ServiceProvider.GetRequiredService<FiszkItDbContext>();

        Services = scope.ServiceProvider;
    }

    protected async Task AssumeEntityInDbAsync<T>(params T[] entities)
    {
        foreach (var entity in entities)
        {
            await _dbContext.AddAsync(entity!);
        }

        await _dbContext.SaveChangesAsync();
    }
}