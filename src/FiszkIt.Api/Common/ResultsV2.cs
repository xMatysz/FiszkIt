using System.Net;
using ErrorOr;

namespace FiszkIt.Api.Common;

public static class ResultsV2
{
    public static IResult Problem(Error error)
    {
        return Results.Problem(
            detail: error.Description,
            title: error.Code,
            statusCode: GetStatusCode(error.Type));
    }

    public static IResult Problem(List<Error> errors)
    {
        var firstError = errors.First();
        return Problem(firstError);
    }

    private static int? GetStatusCode(ErrorType errorType)
        => errorType switch
        {
            ErrorType.NotFound => (int)HttpStatusCode.NotFound,
            ErrorType.Validation => (int)HttpStatusCode.BadRequest,
            ErrorType.Unauthorized => (int)HttpStatusCode.Unauthorized,
            ErrorType.Forbidden => (int)HttpStatusCode.Forbidden,
            ErrorType.Conflict => (int)HttpStatusCode.BadRequest,
            ErrorType.Failure => (int)HttpStatusCode.InternalServerError,
            ErrorType.Unexpected => (int)HttpStatusCode.InternalServerError,
            _ => throw new ArgumentOutOfRangeException(nameof(errorType), errorType, null)
        };
}