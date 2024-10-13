using ErrorOr;

namespace FiszkIt.Domain.FlashCardEntity;

public static class FlashCardErrors
{
    public static Error QuestionCannotBeEmpty
        => Error.Validation(
            code: $"{nameof(FlashCardErrors)}.{nameof(QuestionCannotBeEmpty)}",
            description: "Question cannot be empty");

    public static Error AnswerCannotBeEmpty
        => Error.Validation(
            code: $"{nameof(FlashCardErrors)}.{nameof(AnswerCannotBeEmpty)}",
            description: "Answer cannot be empty");
}
