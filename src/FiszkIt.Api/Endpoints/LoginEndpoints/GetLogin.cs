using FiszkIt.Api.Configuration;
using Microsoft.Extensions.Options;

namespace FiszkIt.Api.Endpoints.LoginEndpoints;

public static class GetLogin
{
    public static IEndpointRouteBuilder MapGetLogin(this IEndpointRouteBuilder app)
    {
        app.MapGet("/getLogin", async (
                IOptions<CognitoOptions> cognitoOptions) =>
            $"{cognitoOptions.Value.DomainUrl}/login?" +
            $"response_type={cognitoOptions.Value.ResponseType}&" +
            $"client_id={cognitoOptions.Value.ClientId}&" +
            $"redirect_uri={cognitoOptions.Value.RedirectUri}");

        return app;
    }
}