using FiszkIt.Domain.FlashCardEntity;
using FiszkIt.Tests.Shared.Builders;
using FluentAssertions;
using Xunit;

namespace FiszkIt.Domain.Tests.Unit;

public class FlashCardsSpecificationTests
{
    [Fact]
    public void Update_Should_UpdateProperties()
    {
        const string newQuestion = "What";
        const string newAnswer = "Yes";
        var initCard = FlashCardBuilder.Default.Build();

        initCard.Update(newQuestion, newAnswer);

        initCard.Question.Should().Be(newQuestion);
        initCard.Answer.Should().Be(newAnswer);
    }

    [Theory]
    [InlineData("", "A")]
    [InlineData("Q", "")]
    [InlineData("", "")]
    [InlineData(" ", " ")]
    [InlineData("", " ")]
    [InlineData(" ", "")]
    public void Update_Fail_WhenPropertyIsEmpty(string question, string answer)
    {
        var initCard = FlashCardBuilder.Default.Build();

        var result = initCard.Update(question, answer);

        result.IsError.Should().BeTrue();
    }

    [Fact]
    public void Create_Should_UpdateProperties()
    {
        const string question = "What";
        const string answer = "Yes";

        var result = FlashCard.Create(question, answer);

        result.IsError.Should().BeFalse();
        result.Value.Question.Should().Be(question);
        result.Value.Answer.Should().Be(answer);
    }

    [Theory]
    [InlineData("", "A")]
    [InlineData("Q", "")]
    [InlineData("", "")]
    [InlineData(" ", " ")]
    [InlineData("", " ")]
    [InlineData(" ", "")]
    public void Create_Fail_WhenPropertyIsEmpty(string question, string answer)
    {
        var result = FlashCard.Create(question, answer);

        result.IsError.Should().BeTrue();
    }
}
