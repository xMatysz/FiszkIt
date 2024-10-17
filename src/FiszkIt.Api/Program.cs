using Amazon.CognitoIdentityProvider;
using Amazon.DynamoDBv2;
using FiszkIt.Api.Configuration;
using FiszkIt.Api.Endpoints.AuthEndpoints;
using FiszkIt.Api.Endpoints.FlashSetEndpoints;
using FiszkIt.Application.Repository;
using FiszkIt.Application.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddAWSLambdaHosting(LambdaEventSource.HttpApi);

    builder.Logging.SetMinimumLevel(LogLevel.Trace);
    var env = builder.Environment.EnvironmentName;
    builder.Configuration.AddSystemsManager(opt =>
    {
        opt.Path = $"/FiszkIt/{env}/";
        opt.ReloadAfter = TimeSpan.FromMinutes(13);
    });

    builder.Services.AddAuthorization();

    builder.Services
        .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer();

    builder.Services.ConfigureOptions<JwtBearerOptionsSetup>();
    builder.Services.ConfigureOptions<CognitoOptionsSetup>();
    builder.Services.ConfigureOptions<ApplicationOptionsSetup>();

    builder.Services.AddHttpClient();

    builder.Services.AddScoped<IAmazonCognitoIdentityProvider, AmazonCognitoIdentityProviderClient>();

    builder.Services.AddScoped<IAmazonDynamoDB, AmazonDynamoDBClient>();
    builder.Services.AddScoped<IFlashSetService, FlashSetService>();
    builder.Services.AddScoped<IFlashSetRepository, FlashSetRepository>();
}

var app = builder.Build();
{
    app.UseAuthentication();
    app.UseAuthorization();

    app
        .RegisterFlashSetEndpoints()
        .RegisterAuthEndpoints();
}

await app.RunAsync();
