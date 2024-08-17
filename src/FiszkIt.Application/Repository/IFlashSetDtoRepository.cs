using System.Text.Json;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using FiszkIt.Domain;
using FiszkIt.Application.Repository.Dtos;

namespace FiszkIt.Application.Repository;

public interface IFlashSetDtoRepository
{
    Task<FlashSetDto?> GetByIdForUserAsync(Guid flashCardId, Guid userId, CancellationToken cancellationToken);
    Task<FlashSetDto[]> GetAllForUserAsync(Guid userId, CancellationToken cancellationToken);
}

public class FlashSetDynamoDbRepository : IFlashSetRepository
{
    public async Task<FlashSetDto> AddAsync(FlashSet flashSet, CancellationToken cancellationToken)
    {
        var client = new AmazonDynamoDBClient();

        var dynamoFlashSetItem = new FlashSetItem(flashSet);
        var cards = dynamoFlashSetItem.FlashCards.Select(x => new AttributeValue
        {
            M = new Dictionary<string, AttributeValue>
            {
                { nameof(x.Id), new AttributeValue { S = x.Id.ToString() } },
                { nameof(x.Question), new AttributeValue { S = x.Question } },
                { nameof(x.Answer), new AttributeValue { S = x.Answer } },
            }
        });

        var putRequest = new PutItemRequest
        {
            TableName = "FiszkIt-Table",
            Item = new Dictionary<string, AttributeValue>
            {
                { nameof(FlashSetItem.PK), new AttributeValue { S = dynamoFlashSetItem.PK } },
                { nameof(FlashSetItem.SK), new AttributeValue { S = dynamoFlashSetItem.SK } },
                { nameof(FlashSetItem.CreatorId), new AttributeValue { S = dynamoFlashSetItem.CreatorId.ToString() } },
                { nameof(FlashSetItem.Name), new AttributeValue { S = dynamoFlashSetItem.Name } },
                {
                    nameof(FlashSetItem.FlashCards), new AttributeValue { L = cards.ToList() }
                },
            }
        };

        await client.PutItemAsync(putRequest, cancellationToken);

        return new FlashSetDto(flashSet);
    }

    public async Task<FlashSetDto> AddAsync(FlashSet flashSet, string url, CancellationToken cancellationToken)
    {
        var client = new AmazonDynamoDBClient(new AmazonDynamoDBConfig { ServiceURL = url });

        var dynamoFlashSetItem = new FlashSetItem(flashSet);
        var cards = dynamoFlashSetItem.FlashCards.Select(x => new AttributeValue
        {
            M = new Dictionary<string, AttributeValue>
            {
                { nameof(x.Id), new AttributeValue { S = x.Id.ToString() } },
                { nameof(x.Question), new AttributeValue { S = x.Question } },
                { nameof(x.Answer), new AttributeValue { S = x.Answer } },
            }
        });

        var putRequest = new PutItemRequest
        {
            TableName = "FiszkIt-Table",
            Item = new Dictionary<string, AttributeValue>
            {
                { nameof(FlashSetItem.PK), new AttributeValue { S = dynamoFlashSetItem.PK } },
                { nameof(FlashSetItem.SK), new AttributeValue { S = dynamoFlashSetItem.SK } },
                { nameof(FlashSetItem.CreatorId), new AttributeValue { S = dynamoFlashSetItem.CreatorId.ToString() } },
                { nameof(FlashSetItem.Name), new AttributeValue { S = dynamoFlashSetItem.Name } },
                {
                    nameof(FlashSetItem.FlashCards), new AttributeValue { L = cards.ToList() }
                },
            }
        };

        await client.PutItemAsync(putRequest, cancellationToken);

        return new FlashSetDto(flashSet);
    }
    public async Task<FlashSet> GetById(Guid userId, Guid flashSetId, CancellationToken cancellationToken)
    {
        var client = new AmazonDynamoDBClient();

        var queryItemRequest = new QueryRequest
        {
            TableName = "FiszkIt-Table",
            KeyConditionExpression = "PK = :pk",
            ExpressionAttributeValues = new Dictionary<string, AttributeValue>
            {
                { ":pk", new AttributeValue { S = $"USER#{userId}" } }
            }
        };
        
        var res = await client.QueryAsync(queryItemRequest, cancellationToken);
        var doc = Document.FromAttributeMap(res.Items.First());
        var set = JsonSerializer.Deserialize<FlashSetItem>(doc.ToJson());
        return null;
    }
    
    public async Task<FlashSet> GetById(Guid userId, Guid flashSetId, string url, CancellationToken cancellationToken)
    {
        var client = new AmazonDynamoDBClient(new AmazonDynamoDBConfig { ServiceURL = url });

        var queryItemRequest = new QueryRequest
        {
            TableName = "FiszkIt-Table",
            KeyConditionExpression = "PK = :pk",
            ExpressionAttributeValues = new Dictionary<string, AttributeValue>
            {
                { ":pk", new AttributeValue { S = $"USER#{userId}" } }
            }
        };
        
        var res = await client.QueryAsync(queryItemRequest, cancellationToken);
        var doc = Document.FromAttributeMap(res.Items.First());
        var set = JsonSerializer.Deserialize<FlashSetItem>(doc.ToJson());
        return null;
    }
}