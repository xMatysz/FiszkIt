using Microsoft.Extensions.Options;

namespace FiszkIt.Api.Configuration;

public class CognitoOptionsSetup(IConfiguration configuration) :
    IConfigureNamedOptions<CognitoOptions>
{
    private const string ConfigurationSectionName = "Cognito";

    public void Configure(CognitoOptions options) =>
        configuration
            .GetSection(ConfigurationSectionName)
            .Bind(options);

    public void Configure(string? name, CognitoOptions options) =>
        Configure(options);
}
