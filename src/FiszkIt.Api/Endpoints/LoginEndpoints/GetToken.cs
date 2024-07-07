using FiszkIt.Api.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using JsonOptions = FiszkIt.Api.Common.JsonOptions;

namespace FiszkIt.Api.Endpoints.LoginEndpoints;

public static class GetToken
{
    private record LoginGetTokenResponse(
        string IdToken,
        string AccessToken,
        string RefreshToken,
        int ExpiresIn,
        string TokenType);

    public static IEndpointRouteBuilder MapGetToken(this IEndpointRouteBuilder app)
    {
        app.MapGet("/token/{codeOrRefreshToken}", async (
            string codeOrRefreshToken,
            IOptions<CognitoOptions> cognitoOptions,
            HttpClient client) =>
        {
            var isCode = Guid.TryParse(codeOrRefreshToken, out _);
            var tokenUrl = $"{cognitoOptions.Value.DomainUrl}/oauth2/token";

            var dic = new Dictionary<string, string>
            {
                { "grant_type", isCode ? "authorization_code" : "refresh_token" },
                { "client_id", cognitoOptions.Value.ClientId },
                { "client_secret", cognitoOptions.Value.ClientSecret },
                { "redirect_uri", cognitoOptions.Value.RedirectUri },
                { isCode ? "code" : "refresh_token", codeOrRefreshToken }
            };

            var responseMessage = await client.PostAsync(tokenUrl, new FormUrlEncodedContent(dic));

            if (!responseMessage.IsSuccessStatusCode)
            {
                var status = responseMessage.StatusCode;
                var problem = new ProblemDetails
                {
                    Title = "Invalid Code or Token",
                    Status = (int)status,
                    Detail = "Code or Token is invalid or code was already used",
                };

                return Results.Problem(problem);
            }

            var tokenResponse =
                await responseMessage.Content.ReadFromJsonAsync<LoginGetTokenResponse>(JsonOptions.Snake);

            return Results.Ok(tokenResponse);
        });

        return app;
    }
}