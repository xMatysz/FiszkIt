using System.Security.Claims;
using ErrorOr;

namespace FiszkIt.Api.Common;

public static class Extensions
{
    public static Guid GetUserId(this HttpContext context)
        => Guid.Parse(context.User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

    public static IResult Problem(this IResult result, List<Error> errors)
    {
        return Results.Problem();
    }
}