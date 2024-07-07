using FiszkIt.Api.Common;
using FiszkIt.Api.Configuration;
using Microsoft.Extensions.Options;

namespace FiszkIt.Api.Endpoints.LoginEndpoints;

public static class GetToken
{
    private class LoginGetTokenResponse(
        string IdToken,
        string AccessToken,
        string RefreshToken,
        int ExpiresIn,
        string TokenType);

    public static IEndpointRouteBuilder MapGetToken(this IEndpointRouteBuilder app)
    {
        app.MapGet("/getToken", async (IOptions<CognitoOptions> cognitoOptions, string code) =>
        {
            var tokenUrl = $"{cognitoOptions.Value.DomainUrl}/oauth2/token";
            var client = new HttpClient();
            var dic = new Dictionary<string, string>
            {
                { "grant_type", "authorization_code" },
                { "client_id", cognitoOptions.Value.ClientId },
                { "client_secret", cognitoOptions.Value.ClientSecret },
                { "redirect_uri", cognitoOptions.Value.RedirectUri },
                { "code", code }
            };
            var res = await client.PostAsync(tokenUrl, new FormUrlEncodedContent(dic));
            var resp = await res.Content.ReadFromJsonAsync<LoginGetTokenResponse>(JsonOptions.Snake);
            return resp;
        });

        return app;
    }
}