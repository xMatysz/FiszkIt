using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace FiszkIt.Api.Configuration;

public class JwtBearerOptionsSetup(IConfiguration configuration, IOptions<CognitoOptions> cognitoOptions) : IConfigureNamedOptions<JwtBearerOptions>
{
    public void Configure(JwtBearerOptions options)
    {
        var region = configuration["AWS_REGION"]!;
        Console.WriteLine($"Auth region: {region}");

        options.IncludeErrorDetails = true;
        options.RequireHttpsMetadata = false;
        options.Authority = $"https://cognito-idp.{region}.amazonaws.com/{cognitoOptions.Value.UserPoolId}";
        options.TokenValidationParameters = new TokenValidationParameters
        {
#pragma warning disable CA5404
            ValidateAudience = false,
#pragma warning restore CA5404
            ValidateIssuerSigningKey = true,
            ValidateIssuer = true,
            ValidIssuer = $"https://cognito-idp.{region}.amazonaws.com/{cognitoOptions.Value.UserPoolId}"
        };
    }

    public void Configure(string? name, JwtBearerOptions options)
    {
        Configure(options);
    }
}
