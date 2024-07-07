using System.Text.Json;
using FiszkIt.Api.Configuration;
using FiszkIt.Api.Responses;
using Microsoft.Extensions.Options;

namespace FiszkIt.Api.Endpoints.LoginEndpoints;

public static class GetToken
{
    public static IEndpointRouteBuilder MapGetToken(this IEndpointRouteBuilder app)
    {
        app.MapGet("/getToken", async (IOptions<CognitoOptions> cognitoOptions, string code) =>
        {
            var tokenUrl = $"{cognitoOptions.Value.DomainUrl}/oauth2/token";
            var client = new HttpClient();
            var dic = new Dictionary<string, string>
            {
                { "grant_type", cognitoOptions.Value.GrantType },
                { "client_id", cognitoOptions.Value.ClientId },
                { "client_secret", cognitoOptions.Value.ClientSecret },
                { "redirect_uri", cognitoOptions.Value.RedirectUri },
                { "code", code }
            };
            var res = await client.PostAsync(tokenUrl, new FormUrlEncodedContent(dic));
            var resp = await res.Content.ReadFromJsonAsync<TokenResponse>(new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
            });
            return resp;
        });

        return app;
    }
}