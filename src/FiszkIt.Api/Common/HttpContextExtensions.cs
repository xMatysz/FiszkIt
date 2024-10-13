using System.Security.Claims;

namespace FiszkIt.Api.Common;

public static class HttpContextExtensions
{
    public static Guid GetUserId(this HttpContext context)
        => Guid.Parse(context.User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
}
