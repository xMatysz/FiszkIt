using Amazon.DynamoDBv2;
using FiszkIt.Api;
using FiszkIt.Application.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Testcontainers.DynamoDb;
using Xunit;

namespace FiszkIt.Application.Tests.Integration;

public class WebAppFactory : WebApplicationFactory<IWebAppMarker>, IAsyncLifetime
{
    private readonly DynamoDbContainer dynamoDbContainer =
        new DynamoDbBuilder()
            .Build();
    public async Task InitializeAsync()
    {
        await dynamoDbContainer.StartAsync();
    }

    public new async Task DisposeAsync()
    {
        await dynamoDbContainer.DisposeAsync().AsTask();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);

        var dynamoConfig = new AmazonDynamoDBConfig
        {
            ServiceURL = dynamoDbContainer.GetConnectionString()
        };

        builder.ConfigureTestServices(collection =>
        {
            collection.RemoveAll<IAmazonDynamoDB>();
            collection.AddScoped<IAmazonDynamoDB>(_ => new AmazonDynamoDBClient(dynamoConfig));

            collection.PostConfigureAll<ApplicationOptions>(opt => opt.TableName = "TESTTABLENAME");
        });
    }
}
