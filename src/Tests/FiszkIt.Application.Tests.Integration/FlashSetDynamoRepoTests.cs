using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using FiszkIt.Application.Repository;
using FiszkIt.Domain;
using Testcontainers.DynamoDb;
using Xunit;

namespace FiszkIt.Application.Tests.Integration;

public class FlashSetDynamoRepoTests : IAsyncLifetime
{
    private DynamoDbContainer _container;

    public FlashSetDynamoRepoTests()
    {
        _container = new DynamoDbBuilder()
            .Build();
    }
    
    [Fact]
    public async Task Test()
    {
        var amazonDbConfig = new AmazonDynamoDBConfig { ServiceURL = _container.GetConnectionString() };
        var client = new AmazonDynamoDBClient(amazonDbConfig);
        var request = new CreateTableRequest
        {
            TableName = "FiszkIt-Table",
            KeySchema = new List<KeySchemaElement>
            {
                new("PK", KeyType.HASH),
                new("SK", KeyType.RANGE)
            },
            AttributeDefinitions = new List<AttributeDefinition>
            {
                new("PK", ScalarAttributeType.S),
                new("SK", ScalarAttributeType.S)
            },
            BillingMode = BillingMode.PAY_PER_REQUEST
        };
        await client.CreateTableAsync(request);
        var repo = new FlashSetDynamoDbRepository();
        var set = FlashSet.Create(Guid.NewGuid(), "BESTSET").Value;
        await repo.AddAsync(set,_container.GetConnectionString(), default);
        var existingItem = await repo.GetById(set.CreatorId, set.Id, _container.GetConnectionString(), default);
    }

    public async Task InitializeAsync()
    {
        await _container.StartAsync();
    }

    public async Task DisposeAsync()
    {
        await _container.DisposeAsync().AsTask();
    }
}