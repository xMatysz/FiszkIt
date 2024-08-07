using ErrorOr;

namespace FiszkIt.Domain;

public static class FlashSetErrors
{
    public static Error NotFound => Error.NotFound(
        "FlashSet.NotFound",
        "Flash set was not found");
}