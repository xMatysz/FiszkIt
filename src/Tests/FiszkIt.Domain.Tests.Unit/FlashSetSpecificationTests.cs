using FluentAssertions;
using Xunit;

namespace FiszkIt.Domain.Tests.Unit;

public class FlashSetSpecificationTests
{
    [Fact]
    public void Ctor_Should_AssignUserIdAsCreator()
    {
        var userId = Guid.NewGuid();

        var flashSet = new FlashSet(userId);

        flashSet.CreatorId.Should().Be(userId);
    }

    [Fact]
    public void AddFlashCard_Should_AddFlashCardToCollection()
    {
        var flashSet = new FlashSet(Guid.NewGuid());
        var flashCard = new FlashCard("q", "a");

        flashSet.AddFlashCard(flashCard);

        flashSet.FlashCards.Should().HaveCount(1);
        flashSet.FlashCards.Single().Should().Be(flashCard);
    }

    [Fact]
    public void RemoveFlashCard_Should_RemoveFlashCardFromCollection()
    {
        var flashSet = new FlashSet(Guid.NewGuid());
        var flashCard = new FlashCard("q", "a");
        flashSet.AddFlashCard(flashCard);

        flashSet.RemoveFlashCard(flashCard);

        flashSet.FlashCards.Should().BeEmpty();
    }
}