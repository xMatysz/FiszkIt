using ErrorOr;
using FiszkIt.Core;
using FiszkIt.Core.Extensions;

namespace FiszkIt.Domain.FlashCardEntity;

public class FlashCard : Entity
{
    public string Question { get; set; }
    public string Answer { get; set; }

    public FlashCard()
        : this(string.Empty, string.Empty, Guid.Empty)
    {
    }

    private FlashCard(string question, string answer, Guid? id = null)
        : base(id ?? Guid.NewGuid())
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

    public static FlashCard CreatePerfect(Guid id, string question, string answer) =>
        new(question, answer, id);

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
}
