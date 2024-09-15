using Amazon.DynamoDBv2.Model;
using ErrorOr;
using FiszkIt.Application.Repository;
using FiszkIt.Application.Repository.Dtos;
using FiszkIt.Application.Repository.Items;
using FiszkIt.Domain.FlashCardEntity;
using FiszkIt.Domain.FlashSetEntity;
using FiszkIt.Shared.Tests.Builders;
using FiszkIt.Tests.Shared.Builders;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace FiszkIt.Application.Tests.Integration.Repositories;

public class WhenUsingFlashSetRepository : IntegrationTestsBase
{
    private readonly IFlashSetRepository _flashSetRepository;

    public WhenUsingFlashSetRepository(WebAppFactory factory)
        : base(factory)
    {
        _flashSetRepository = ServiceProvider.GetRequiredService<IFlashSetRepository>();
    }

    [Fact]
    public async Task GetById_ReturnEntry()
    {
        var flashSet = new FlashSetBuilder()
            .WithFlashCards(FlashCardBuilder.Default.Build())
            .Build();

        await PutItemIntoDynamoDb(flashSet);

        var result = await _flashSetRepository.GetForUserById(flashSet.CreatorId, flashSet.Id, CancellationToken.None);

        result.IsError.Should().BeFalse();
        var set = result.Value;
        set.Id.Should().Be(flashSet.Id);
        set.CreatorId.Should().Be(flashSet.CreatorId);
        set.Name.Should().Be(flashSet.Name);
        set.FlashCards.Should().BeEquivalentTo(flashSet.FlashCards.Select(x => new FlashCardDto(x)));
    }

    [Fact]
    public async Task Add_ShouldPutEntryIntoDb()
    {
        var flashSet = new FlashSetBuilder()
            .WithFlashCards(FlashCardBuilder.Default.Build())
            .Build();

        await _flashSetRepository.AddAsync(flashSet.CreatorId, flashSet, CancellationToken.None);

        var request = new ScanRequest { TableName = ApplicationOptions.Value.TableName };
        var result = await DynamoDbClient.ScanAsync(request);
        result.Items.Should().ContainSingle(item => ValidateItem(item, flashSet));
    }

    [Fact]
    public async Task Add_Fail_WhenItemAlreadyExist()
    {
        var flashSet = new FlashSetBuilder()
            .WithFlashCards(FlashCardBuilder.Default.Build())
            .Build();

        await PutItemIntoDynamoDb(flashSet);

        var result = await _flashSetRepository.AddAsync(flashSet.CreatorId, flashSet, CancellationToken.None);
        result.IsError.Should().BeTrue();
        result.FirstError.Should().Be(FlashSetErrors.AlreadyExist);
    }

    private bool ValidateItem(Dictionary<string, AttributeValue> item, FlashSet expectedSet)
    {
        return item[nameof(FlashSet.Id)].S == expectedSet.Id.ToString() &&
               item[nameof(FlashSet.CreatorId)].S == expectedSet.CreatorId.ToString() &&
               item[nameof(FlashSet.Name)].S == expectedSet.Name &&
               ValidateCards(item[nameof(FlashSet.FlashCards)].L, expectedSet.FlashCards);
    }

    private bool ValidateCards(List<AttributeValue> attributeValues, IReadOnlyCollection<FlashCard> expectedSetFlashCards)
    {
        return attributeValues.All(attribute =>
        {
            var cardItem = attribute.M;
            var card = expectedSetFlashCards.First(c => c.Id.ToString() == cardItem[nameof(FlashCard.Id)].S);

            return cardItem[nameof(FlashCard.Question)].S == card.Question &&
                    cardItem[nameof(FlashCard.Answer)].S == card.Answer;
        });
    }

    [Fact]
    public async Task Add_Success_WhenFlashCardListIsEmpty()
    {
        var flashSet = new FlashSetBuilder()
            .Build();

        var act = () => _flashSetRepository.AddAsync(flashSet.CreatorId, flashSet, CancellationToken.None);

        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task GetAll_Return_AllUserFlashSets()
    {
        var userId = Guid.NewGuid();

        FlashSet[] userEntries =
        [
            FlashSetBuilder.Default().WithCreatorId(userId).Build(),
            FlashSetBuilder.Default().WithCreatorId(userId).Build(),
            FlashSetBuilder.Default().WithCreatorId(userId).Build(),
        ];

        var otherFlashSets = Enumerable
            .Range(0, 100)
            .Select(_ => FlashSetBuilder.Default().Build())
            .ToArray();

        var concatSets = userEntries
            .Concat(otherFlashSets)
            .ToArray();
        Random.Shared.Shuffle(concatSets);

        await Task.WhenAll(concatSets.Select(PutItemIntoDynamoDb));

        var setsFromDb = await _flashSetRepository.GetAllForUser(userId, CancellationToken.None);

        setsFromDb.Should().HaveCount(userEntries.Length);
        setsFromDb.Select(x => x.Id)
            .Should()
            .BeEquivalentTo(userEntries.Select(x => x.Id));
    }

    [Fact]
    public async Task Remove_Should_DeleteItem()
    {
        var flashSet = FlashSetBuilder.Default().Build();
        await PutItemIntoDynamoDb(flashSet);

        var result = await _flashSetRepository.RemoveAsync(flashSet.CreatorId, flashSet.Id, CancellationToken.None);

        result.Value.Should().Be(Result.Deleted);

        var request = new ScanRequest { TableName = ApplicationOptions.Value.TableName };
        var scanResult = await DynamoDbClient.ScanAsync(request);
        scanResult.Items.Should().BeEmpty();
    }

    [Fact]
    public async Task Remove_Failed_WhenItemNotExist()
    {
        var flashSet = FlashSetBuilder.Default().Build();

        var result = await _flashSetRepository.RemoveAsync(flashSet.CreatorId, flashSet.Id, CancellationToken.None);

        result.IsError.Should().BeTrue();
    }

    private async Task PutItemIntoDynamoDb(FlashSet flashSet)
    {
        var cards = flashSet.FlashCards.Select(x => new AttributeValue
        {
            M = new Dictionary<string, AttributeValue>
            {
                { nameof(x.Id), new AttributeValue { S = x.Id.ToString() } },
                { nameof(x.Question), new AttributeValue { S = x.Question } },
                { nameof(x.Answer), new AttributeValue { S = x.Answer } },
            }
        });

        var request = new PutItemRequest
        {
            TableName = ApplicationOptions.Value.TableName,
            Item = new Dictionary<string, AttributeValue>
            {
                { nameof(FlashSetItem.PK), new AttributeValue { S = FlashSetItem.CreatePk(flashSet.CreatorId) } },
                { nameof(FlashSetItem.SK), new AttributeValue { S = FlashSetItem.CreateSk(flashSet.Id) } },
                { nameof(FlashSet.Id), new AttributeValue { S = flashSet.Id.ToString() } },
                { nameof(FlashSet.CreatorId), new AttributeValue { S = flashSet.CreatorId.ToString() } },
                { nameof(FlashSet.Name), new AttributeValue { S = flashSet.Name } },
                { nameof(FlashSet.FlashCards), new AttributeValue { L = cards.ToList() , IsLSet = true} },
            }
        };

        await DynamoDbClient.PutItemAsync(request);
    }
}