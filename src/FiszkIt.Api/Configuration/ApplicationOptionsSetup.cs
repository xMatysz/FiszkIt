using FiszkIt.Application.Configuration;
using Microsoft.Extensions.Options;

namespace FiszkIt.Api.Configuration;

public class ApplicationOptionsSetup(IConfiguration configuration) : IConfigureNamedOptions<ApplicationOptions>
{
    public void Configure(ApplicationOptions options)
    {
        configuration.GetSection(nameof(ApplicationOptions)).Bind(options);
    }

    public void Configure(string? name, ApplicationOptions options)
    {
        Configure(options);
    }
}
