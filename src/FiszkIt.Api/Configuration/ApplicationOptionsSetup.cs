using FiszkIt.Application.Configuration;
using Microsoft.Extensions.Options;

namespace FiszkIt.Api.Configuration;

public class ApplicationOptionsSetup(IConfiguration configuration) : IConfigureNamedOptions<ApplicationOptions>
{
    private const string SectionName = "Application";

    public void Configure(ApplicationOptions options)
    {
        configuration.GetSection(SectionName).Bind(options);
    }

    public void Configure(string? name, ApplicationOptions options)
    {
        Configure(options);
    }
}
