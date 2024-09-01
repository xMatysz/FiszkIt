using Amazon.DynamoDBv2;
using FiszkIt.Api.Configuration;
using FiszkIt.Api.Endpoints.FlashSetEndpoints;
using FiszkIt.Api.Endpoints.LoginEndpoints;
using FiszkIt.Application.Repository;
using FiszkIt.Application.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddAWSLambdaHosting(LambdaEventSource.HttpApi);

    builder.Services.AddAuthorization();
    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer();

    builder.Services.ConfigureOptions<JwtBearerOptionsSetup>();
    builder.Services.ConfigureOptions<CognitoOptionsSetup>();
    builder.Services.ConfigureOptions<ApplicationOptionsSetup>();

    builder.Services.AddHttpClient();

    builder.Services.AddScoped<IAmazonDynamoDB, AmazonDynamoDBClient>();
    builder.Services.AddScoped<IFlashSetService, FlashSetService>();
    builder.Services.AddScoped<IFlashSetRepository, FlashSetRepository>();
}

var app = builder.Build();
{
    app.UseAuthentication();
    app.UseAuthorization();

    app
        .RegisterLoginEndpoints()
        .RegisterFlashSetEndpoints();
}

app.Run();