using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using FiszkIt.Application.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xunit;

namespace FiszkIt.Application.Tests.Integration;

public class IntegrationTestsBase : IClassFixture<WebAppFactory>, IAsyncLifetime, IDisposable
{
    private readonly IServiceScope _scope;

    private static CreateTableRequest GetCreateTableRequest(string tableName)
        => new()
        {
            TableName = tableName,
            BillingMode = BillingMode.PAY_PER_REQUEST,
            KeySchema =
            [
                new KeySchemaElement { AttributeName = "PK", KeyType = KeyType.HASH },
                new KeySchemaElement { AttributeName = "SK", KeyType = KeyType.RANGE }
            ],
            AttributeDefinitions =
            [
                new AttributeDefinition { AttributeName = "PK", AttributeType = ScalarAttributeType.S },
                new AttributeDefinition { AttributeName = "SK", AttributeType = ScalarAttributeType.S }
            ]
        };

    protected IServiceProvider ServiceProvider { get; set; }
    protected IAmazonDynamoDB DynamoDbClient { get; set; }

    protected IOptions<ApplicationOptions> ApplicationOptions { get; set; }

    protected IntegrationTestsBase(WebAppFactory factory)
    {
        _scope = factory.Services.CreateScope();
        ServiceProvider = _scope.ServiceProvider;

        DynamoDbClient = ServiceProvider.GetRequiredService<IAmazonDynamoDB>();
        ApplicationOptions = ServiceProvider.GetRequiredService<IOptions<ApplicationOptions>>();
    }

    public Task InitializeAsync() =>
        DynamoDbClient.CreateTableAsync(GetCreateTableRequest(ApplicationOptions.Value.TableName));

    public Task DisposeAsync() =>
        DynamoDbClient.DeleteTableAsync(ApplicationOptions.Value.TableName);

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing) =>
        _scope.Dispose();
}
