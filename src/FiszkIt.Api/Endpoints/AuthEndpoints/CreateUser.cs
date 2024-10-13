using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using FiszkIt.Api.Configuration;
using Microsoft.Extensions.Options;

namespace FiszkIt.Api.Endpoints.AuthEndpoints;

public static class CreateUser
{
    public static IEndpointRouteBuilder MapCreateUser(this IEndpointRouteBuilder app)
    {
        app.MapPost("/register", async (
            string email,
            IOptions<CognitoOptions> cognitoOptions,
            IAmazonCognitoIdentityProvider cognitoProvider) =>
        {
            var request = new AdminCreateUserRequest
            {
                Username = email,
                UserPoolId = cognitoOptions.Value.UserPoolId
            };

            var response = await cognitoProvider.AdminCreateUserAsync(request);

            return Results.Ok(response);
        });

        return app;
    }
}
