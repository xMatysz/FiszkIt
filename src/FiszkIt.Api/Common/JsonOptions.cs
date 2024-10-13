using System.Text.Json;

namespace FiszkIt.Api.Common;

public static class JsonOptions
{
    public static JsonSerializerOptions Snake => new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
    };
}
