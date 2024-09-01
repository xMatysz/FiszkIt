using FiszkIt.Domain;

namespace FiszkIt.Tests.Shared.Builders;

public class FlashCardBuilder
{
    public static FlashCardBuilder Default => new();

    public static string DefaultQuestion => "WHERE";
    public static string DefaultAnswer => "THERE";

    private string _question = DefaultQuestion;
    private string _answer = DefaultAnswer;

    public FlashCardBuilder WithQuestion(string question)
    {
        _question = question;
        return this;
    }

    public FlashCardBuilder WithAnswer(string answer)
    {
        _answer = answer;
        return this;
    }

    public FlashCard Build()
    {
        return new FlashCard(_question, _answer);
    }

}