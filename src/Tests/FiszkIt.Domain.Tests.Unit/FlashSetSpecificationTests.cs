using FiszkIt.Domain.FlashSetEntity;
using FiszkIt.Tests.Shared.Builders;
using FluentAssertions;
using Xunit;

namespace FiszkIt.Domain.Tests.Unit;

public class FlashSetSpecificationTests
{
    [Fact]
    public void Create_Should_SetCreatorId()
    {
        var userId = Guid.NewGuid();

        var flashSet = FlashSet.Create(userId, "name").Value;

        flashSet.CreatorId.Should().Be(userId);
    }

    [Fact]
    public void Create_Should_FailWhenNameIsEmpty()
    {
        const string emptyString = "";

        var flashSet = FlashSet.Create(Guid.NewGuid(), emptyString);

        flashSet.IsError.Should().BeTrue();
    }

    [Fact]
    public void Create_Should_SetName()
    {
        const string name = "TestName";

        var flashSet = FlashSet.Create(Guid.NewGuid(), name).Value;

        flashSet.Name.Should().Be(name);
    }

    [Fact]
    public void AddFlashCard_Should_AddFlashCardToCollection()
    {
        var flashSet = FlashSetBuilder.Default().Build();
        var flashCard = FlashCardBuilder.Default.Build();

        flashSet.AddFlashCard(flashCard);

        flashSet.FlashCards.Should().HaveCount(1);
        flashSet.FlashCards.Single().Should().Be(flashCard);
    }

    [Fact]
    public void RemoveFlashCard_Should_RemoveFlashCardFromCollection()
    {
        var flashCard = FlashCardBuilder.Default.Build();
        var flashSet = FlashSetBuilder.Default().WithFlashCards(flashCard).Build();

        flashSet.RemoveFlashCard(flashCard.Id);

        flashSet.FlashCards.Should().BeEmpty();
    }
}
