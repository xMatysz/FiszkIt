using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;

namespace FiszkIt.Api.Configuration;

public class JwtBearerOptionsSetup(IConfiguration configuration) : IConfigureNamedOptions<JwtBearerOptions>
{
    private const string ConfigurationSectionName = "JwtBearer";

    public void Configure(JwtBearerOptions options)
    {
        configuration.GetSection(ConfigurationSectionName).Bind(options);
    }

    public void Configure(string? name, JwtBearerOptions options)
    {
        Configure(options);
    }
}
