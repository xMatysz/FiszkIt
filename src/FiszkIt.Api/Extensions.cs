using System.Security.Claims;

namespace FiszkIt.Api;

public static class Extensions
{
    public static string GetUserId(this HttpContext context)
        => context.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
}