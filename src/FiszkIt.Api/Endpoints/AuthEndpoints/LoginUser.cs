using System.Net;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using FiszkIt.Api.Configuration;
using Microsoft.Extensions.Options;

namespace FiszkIt.Api.Endpoints.AuthEndpoints;

public static class LoginUser
{
    //TODO: exeptions
    public static IEndpointRouteBuilder MapLoginUser(this IEndpointRouteBuilder app)
    {
        app.MapPost("/login", async (
            string? email,
            string? password,
            string? token,
            IOptions<CognitoOptions> cognitoOptions,
            IAmazonCognitoIdentityProvider cognitoProvider) =>
        {
            AuthFlowType flowType;
            var authParameters = new Dictionary<string, string>();

            if (token is not null)
            {
                flowType = AuthFlowType.REFRESH_TOKEN_AUTH;
                authParameters.Add("REFRESH_TOKEN", token);
            }
            else
            {
                flowType = AuthFlowType.ADMIN_USER_PASSWORD_AUTH;
                authParameters.Add("USERNAME", email);
                authParameters.Add("PASSWORD", password);
            }

            var initAuthRequest = new AdminInitiateAuthRequest
            {
                AuthFlow = flowType,
                ClientId = cognitoOptions.Value.ClientId,
                UserPoolId = cognitoOptions.Value.UserPoolId,
                AuthParameters = authParameters
            };

            var response = await cognitoProvider.AdminInitiateAuthAsync(initAuthRequest);

            if (response.ChallengeName is not null && response.HttpStatusCode == HttpStatusCode.OK)
            {
                var updateRequest = new AdminUpdateUserAttributesRequest
                {
                    UserPoolId = cognitoOptions.Value.UserPoolId,
                    Username = email,
                    UserAttributes =
                    [
                        new AttributeType { Name = "email_verified", Value = "true" }
                    ]
                };

                await cognitoProvider.AdminUpdateUserAttributesAsync(updateRequest);
            }

            return Results.Ok(response);
        });

        return app;
    }
}