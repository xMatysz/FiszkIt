using FiszkIt.Api;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Npgsql;
using Respawn;
using Respawn.Graph;
using Testcontainers.PostgreSql;
using Xunit;

namespace FiszkIt.Application.Tests.Integration;

public class WebAppFactory : WebApplicationFactory<IWebAppMarker>, IAsyncLifetime
{
    private Respawner _respawner = default!;
    private NpgsqlConnection _dbConnection = default!;
    private readonly PostgreSqlContainer _dbContainer =
        new PostgreSqlBuilder()
            .WithDatabase("fiszkit")
            .WithUsername("postgres")
            .WithPassword("123")
            .Build();

    public Task ResetDbAsync() => _respawner.ResetAsync(_dbConnection);

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
        await MigrateDbAsync();
        await InitializeRespawnAsync();
    }

    public new async Task DisposeAsync()
    {
        await _dbContainer.DisposeAsync().AsTask();
    }

    private async Task MigrateDbAsync()
    {
        using var scope = Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<FiszkItDbContext>();
        await dbContext.Database.MigrateAsync();
    }

    private async Task InitializeRespawnAsync()
    {
        _dbConnection = new NpgsqlConnection(_dbContainer.GetConnectionString());

        await _dbConnection.OpenAsync();

        _respawner = await Respawner.CreateAsync(_dbConnection, new RespawnerOptions
        {
            DbAdapter = DbAdapter.Postgres,
            SchemasToInclude = ["public"],
            TablesToIgnore = [new Table("__EFMigrationsHistory")]
        });
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);

        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll(typeof(DbContextOptions<FiszkItDbContext>));
            services.AddDbContext<FiszkItDbContext>(opt =>
                opt.UseNpgsql(_dbContainer.GetConnectionString() + ";Include Error Detail=true;"));
        });
    }

}