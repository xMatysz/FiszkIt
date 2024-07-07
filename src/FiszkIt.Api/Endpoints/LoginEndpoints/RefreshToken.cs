using System.Text.Json;
using FiszkIt.Api.Configuration;
using FiszkIt.Api.Responses;
using Microsoft.Extensions.Options;

namespace FiszkIt.Api.Endpoints.LoginEndpoints;

public static class RefreshToken
{
    public static IEndpointRouteBuilder MapRefreshToken(this IEndpointRouteBuilder app)
    {

        app.MapGet("/refreshToken", async (IOptions<CognitoOptions> cognitoOptions, string refreshToken) =>
        {
            var tokenUrl = $"{cognitoOptions.Value.DomainUrl}/oauth2/token";
            var client = new HttpClient();
            var dic = new Dictionary<string, string>
            {
                { "grant_type", "refresh_token" },
                { "client_id", cognitoOptions.Value.ClientId },
                { "client_secret", cognitoOptions.Value.ClientSecret },
                { "redirect_uri", cognitoOptions.Value.RedirectUri },
                { "refresh_token", refreshToken }
            };
            var res = await client.PostAsync(tokenUrl, new FormUrlEncodedContent(dic));
            var resp = await res.Content.ReadFromJsonAsync<TokenResponse>(new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
            });
            return resp;
        });

        return app;

        return app;
    }
}