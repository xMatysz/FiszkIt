namespace FiszkIt.Api.Configuration;

public class CognitoOptions
{
    public string GrantType { get; set; }
    public string ClientId { get; set; }
    public string ClientSecret { get; set; }
    public string RedirectUri { get; set; }
    public string DomainUrl { get; set; }
    public string ResponseType { get; set; }
}