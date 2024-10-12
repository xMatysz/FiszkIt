using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using FiszkIt.Api.Configuration;
using Microsoft.Extensions.Options;

namespace FiszkIt.Api.Endpoints.AuthEndpoints;

public static class ConfirmUser
{
    public static IEndpointRouteBuilder MapConfirmUser(this IEndpointRouteBuilder app)
    {
        app.MapPost("confirm", async (
            string session,
            string email,
            string password,
            IOptions<CognitoOptions> cognitoOption,
            IAmazonCognitoIdentityProvider cognitoProvider) =>
        {
            var request = new AdminRespondToAuthChallengeRequest
            {
                ClientId = cognitoOption.Value.ClientId,
                UserPoolId = cognitoOption.Value.UserPoolId,
                ChallengeName = ChallengeNameType.NEW_PASSWORD_REQUIRED,
                Session = session,
                ChallengeResponses = new Dictionary<string, string>
                {
                    { "USERNAME", email },
                    { "NEW_PASSWORD", password },
                }
            };

            var response = await cognitoProvider.AdminRespondToAuthChallengeAsync(request);

            return Results.Ok(response);
        });

        return app;
    }
}