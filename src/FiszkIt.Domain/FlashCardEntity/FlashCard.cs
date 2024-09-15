using ErrorOr;
using FiszkIt.Core;
using FiszkIt.Core.Extensions;

namespace FiszkIt.Domain.FlashCardEntity;

public class FlashCard : Entity
{
    public string Question { get; set; }
    public string Answer { get; set; }

    private FlashCard(string question, string answer)
        : base(Guid.NewGuid())
    {
        Question = question;
        Answer = answer;
    }

    public static ErrorOr<FlashCard> Create(string question, string answer)
    {
        var validationResult = ValidateInput(question, answer);

        if (validationResult.IsError)
        {
            return validationResult.Errors;
        }

        return new FlashCard(question, answer);
    }

    public ErrorOr<Updated> Update(string question, string answer)
    {
        var validationResult = ValidateInput(question, answer);

        if (validationResult.IsError)
        {
            return validationResult.Errors;
        }

        Question = question;
        Answer = answer;

        return Result.Updated;
    }

    private static ErrorOr<Success> ValidateInput(string question, string answer)
    {
        var errors = new List<Error>();

        if (question.IsNullOrEmpty())
        {
            errors.Add(FlashCardErrors.QuestionCannotBeEmpty);
        }

        if (answer.IsNullOrEmpty())
        {
            errors.Add(FlashCardErrors.AnswerCannotBeEmpty);
        }

        return errors.Count == 0 ? Result.Success : errors;
    }

    public FlashCard()
    {
    }

    private FlashCard(Guid id, string question, string answer)
        : base(id)
    {
        Question = question;
        Answer = answer;
    }

    public static FlashCard CreatePerfect(Guid id, string question, string answer)
    {
        return new FlashCard(id, question, answer);
    }
}