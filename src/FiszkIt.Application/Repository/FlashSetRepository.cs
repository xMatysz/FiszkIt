using System.Text.Json;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using ErrorOr;
using FiszkIt.Application.Configuration;
using FiszkIt.Application.Repository.Dtos;
using FiszkIt.Application.Repository.Items;
using FiszkIt.Domain.FlashCardEntity;
using FiszkIt.Domain.FlashSetEntity;
using Microsoft.Extensions.Options;

namespace FiszkIt.Application.Repository;

public interface IFlashSetRepository
{
    Task<ErrorOr<FlashSetDto>> AddAsync(Guid userId, FlashSet flashSet, CancellationToken cancellationToken);
    Task<ErrorOr<FlashSetDto>> GetForUserById(Guid userId, Guid flashSetId, CancellationToken cancellationToken);
    Task<FlashSetDto[]> GetAllForUser(Guid userId, CancellationToken cancellationToken);
    Task<ErrorOr<Deleted>> RemoveAsync(Guid userId, Guid flashSetId, CancellationToken cancellationToken);
    Task<ErrorOr<Updated>> UpdateAsync(Guid userId, FlashSet flashSet, CancellationToken cancellationToken);
}

public class FlashSetRepository : IFlashSetRepository
{
    private readonly IAmazonDynamoDB _client;
    private readonly IOptions<ApplicationOptions> _appOptions;

    public FlashSetRepository(IAmazonDynamoDB client, IOptions<ApplicationOptions> appOptions)
    {
        _client = client;
        _appOptions = appOptions;
    }

    public async Task<ErrorOr<FlashSetDto>> AddAsync(Guid userId, FlashSet flashSet, CancellationToken cancellationToken)
    {
        var item = new FlashSetItem(userId, flashSet);

        var cards = MapCards(item.FlashCards);

        var request = new PutItemRequest
        {
            TableName = _appOptions.Value.TableName,
            Item = new Dictionary<string, AttributeValue>
            {
                { nameof(FlashSetItem.PK), new AttributeValue { S = item.PK } },
                { nameof(FlashSetItem.SK), new AttributeValue { S = item.SK } },
                { nameof(FlashSetItem.Id), new AttributeValue { S = item.Id.ToString() } },
                { nameof(FlashSetItem.CreatorId), new AttributeValue { S = item.CreatorId.ToString() } },
                { nameof(FlashSetItem.Name), new AttributeValue { S = item.Name } },
                { nameof(FlashSetItem.FlashCards), new AttributeValue { L = cards.ToList(), IsLSet = true} },
            },
            ConditionExpression = "attribute_not_exists(PK) and attribute_not_exists(SK)"
        };

        try
        {
            await _client.PutItemAsync(request, cancellationToken);
            return new FlashSetDto(item);
        }
        catch (ConditionalCheckFailedException)
        {
            return FlashSetErrors.AlreadyExist;
        }
    }

    private static IEnumerable<AttributeValue> MapCards(FlashCard[] flashCards)
    {
        return flashCards.Select(x => new AttributeValue
        {
            M = new Dictionary<string, AttributeValue>
            {
                { nameof(x.Id), new AttributeValue { S = x.Id.ToString() } },
                { nameof(x.Question), new AttributeValue { S = x.Question } },
                { nameof(x.Answer), new AttributeValue { S = x.Answer } },
            }
        });
    }

    public async Task<ErrorOr<FlashSetDto>> GetForUserById(Guid userId, Guid flashSetId, CancellationToken cancellationToken)
    {
        var pk = FlashSetItem.CreatePk(userId);
        var sk = FlashSetItem.CreateSk(flashSetId);

        var request = new GetItemRequest
        {
            TableName =  _appOptions.Value.TableName,
            Key = new Dictionary<string, AttributeValue>
            {
                { nameof(FlashSetItem.PK), new AttributeValue { S = pk } },
                { nameof(FlashSetItem.SK), new AttributeValue { S = sk } }
            }
        };

        var response = await _client.GetItemAsync(request, cancellationToken);

        return !response.IsItemSet
            ? FlashSetErrors.NotFound
            : ConvertToDto(response.Item);
    }

    public async Task<FlashSetDto[]> GetAllForUser(Guid userId, CancellationToken cancellationToken)
    {
        var queryRequest = new QueryRequest
        {
            TableName = _appOptions.Value.TableName,
            KeyConditionExpression = "#PK = :pk",
            ExpressionAttributeNames = new Dictionary<string, string>
            {
                { "#PK", "PK" }
            },
            ExpressionAttributeValues = new Dictionary<string, AttributeValue>
            {
                { ":pk", new AttributeValue { S = FlashSetItem.CreatePk(userId) } }
            }
        };

        var response = await _client.QueryAsync(queryRequest, cancellationToken);

        return response.Items
            .Select(ConvertToDto)
            .ToArray();
    }

    public async Task<ErrorOr<Deleted>> RemoveAsync(Guid userId, Guid flashSetId, CancellationToken cancellationToken)
    {
        var request = new DeleteItemRequest
        {
            TableName = _appOptions.Value.TableName,
            Key = new()
            {
                { nameof(FlashSetItem.PK), new AttributeValue { S = FlashSetItem.CreatePk(userId) } },
                { nameof(FlashSetItem.SK), new AttributeValue { S = FlashSetItem.CreateSk(flashSetId) } },
            },
            ConditionExpression = "attribute_exists(PK) and attribute_exists(SK)"
        };

        try
        {
            await _client.DeleteItemAsync(request, cancellationToken);
            return Result.Deleted;
        }
        catch (ConditionalCheckFailedException)
        {
            return FlashSetErrors.NotExist;
        }
    }

    public async Task<ErrorOr<Updated>> UpdateAsync(Guid userId, FlashSet flashSet, CancellationToken cancellationToken)
    {
        var item = new FlashSetItem(userId, flashSet);

        var cards = MapCards(item.FlashCards);

        var request = new PutItemRequest
        {
            TableName = _appOptions.Value.TableName,
            Item = new Dictionary<string, AttributeValue>
            {
                { nameof(FlashSetItem.PK), new AttributeValue { S = item.PK } },
                { nameof(FlashSetItem.SK), new AttributeValue { S = item.SK } },
                { nameof(FlashSetItem.Id), new AttributeValue { S = item.Id.ToString() } },
                { nameof(FlashSetItem.CreatorId), new AttributeValue { S = item.CreatorId.ToString() } },
                { nameof(FlashSetItem.Name), new AttributeValue { S = item.Name } },
                { nameof(FlashSetItem.FlashCards), new AttributeValue { L = cards.ToList(), IsLSet = true} },
            },
            ConditionExpression = "attribute_exists(PK) and attribute_exists(SK)"
        };

        try
        {
            await _client.PutItemAsync(request, cancellationToken);
            return Result.Updated;
        }
        catch (ConditionalCheckFailedException)
        {
            return FlashSetErrors.NotExist;
        }
    }

    private static FlashSetDto ConvertToDto(Dictionary<string, AttributeValue> item)
    {
        var document = Document.FromAttributeMap(item);
        var flashSetItem = JsonSerializer.Deserialize<FlashSetItem>(document.ToJson())!;
        return new FlashSetDto(flashSetItem);
    }
}