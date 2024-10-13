using ErrorOr;

namespace FiszkIt.Domain.FlashSetEntity;

public static class FlashSetErrors
{
    public static Error NotFound => Error.NotFound(
        "FlashSet.NotFound",
        "Flash set was not found");

    public static Error AlreadyExist => Error.Conflict(
        "FlashSet.AlreadyExist",
        "Flash set already exist");

    public static Error NameCannotBeEmpty
        => Error.Validation(
            code: $"{nameof(FlashSetErrors)}.{nameof(NameCannotBeEmpty)}",
            description: "Name can't be empty");

    public static Error NotExist
        => Error.NotFound(
            code: $"{nameof(FlashSetErrors)}.{nameof(NotExist)}",
            description: "Flash set not exist");
}
