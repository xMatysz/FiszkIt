using System.Text.Json;
using FiszkIt.Api;
using FiszkIt.Api.Common;
using FiszkIt.Api.Configuration;
using FiszkIt.Api.Responses;
using FiszkIt.Domain;
using FiszkIt.Infrastructure;
using FiszkIt.Infrastructure.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddAWSLambdaHosting(LambdaEventSource.HttpApi);

    builder.Services.AddAuthorization();
    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer();

    builder.Services.ConfigureOptions<JwtBearerOptionsConfigure>();
    builder.Services.ConfigureOptions<CognitoOptionsConfigure>();

    builder.Services.AddScoped<IFlashSetRepository, FlashSetRepository>();

    builder.Services.AddDbContext<FiszkItDbContext>(
        opt => opt.UseNpgsql(builder.Configuration.GetSection("connectionString").Value));
}

var app = builder.Build();
{
    app.UseAuthentication();
    app.UseAuthorization();

    RunMigration(app);
}
{
    app.MapGet("/getLogin", async (IOptions<CognitoOptions> cognitoOptions) =>
        $"{cognitoOptions.Value.DomainUrl}/login?response_type={cognitoOptions.Value.ResponseType}&client_id={cognitoOptions.Value.ClientId}&redirect_uri={cognitoOptions.Value.RedirectUri}");

    app.MapGet("/getToken", async (IOptions<CognitoOptions> cognitoOptions, string code) =>
    {
        var tokenUrl = $"{cognitoOptions.Value.DomainUrl}/oauth2/token";
        var client = new HttpClient();
        var dic = new Dictionary<string, string>
        {
            { "grant_type", cognitoOptions.Value.GrantType },
            { "client_id", cognitoOptions.Value.ClientId },
            { "client_secret", cognitoOptions.Value.ClientSecret },
            { "redirect_uri", cognitoOptions.Value.RedirectUri },
            { "code", code }
        };
        var res = await client.PostAsync(tokenUrl, new FormUrlEncodedContent(dic));
        var resp = await res.Content.ReadFromJsonAsync<TokenResponse>(new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
        });
        return resp;
    });

    app.MapGet("/flashSets", async (
        HttpContext context,
        IFlashSetRepository repository,
        CancellationToken cancellationToken) =>
    {
        var flashSets = await repository.GetAllForUser(context.GetUserId(), cancellationToken);

        return flashSets.Select(f=>new GetFlashSetResponse(f));
    }).RequireAuthorization();

    app.MapPost("/flashSets", async (
        [FromBody] CreateFlashSetRequest request,
        HttpContext context,
        IFlashSetRepository repository,
        FiszkItDbContext dbContext,
        CancellationToken cancellationToken) =>
    {
        var flashSet = FlashSet.Create(context.GetUserId(), request.Name);

        await repository.AddAsync(flashSet.Value, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);
    }).RequireAuthorization();
}

app.Run();

static void RunMigration(IHost app)
{
    using var scope = app.Services.CreateScope();
    using var dbContext = scope.ServiceProvider.GetRequiredService<FiszkItDbContext>();
    dbContext.Database.Migrate();
}