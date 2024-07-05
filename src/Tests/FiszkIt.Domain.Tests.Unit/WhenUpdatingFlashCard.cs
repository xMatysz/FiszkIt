using FluentAssertions;
using Xunit;

namespace FiszkIt.Domain.Tests.Unit;

public class WhenUpdatingFlashCard
{
    [Fact]
    public void Update_Should_UpdateProperties()
    {
        var newQuestion = "What";
        var newAnswer = "Yes";
        var initCard = new FlashCard("q", "a");

        initCard.Update(newQuestion, newAnswer);

        initCard.Question.Should().Be(newQuestion);
        initCard.Answer.Should().Be(newAnswer);
    }
}