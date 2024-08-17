using FiszkIt.Api.Configuration;
using FiszkIt.Api.Endpoints.FlashCardEndpoints;
using FiszkIt.Api.Endpoints.FlashSetEndpoints;
using FiszkIt.Api.Endpoints.LoginEndpoints;
using FiszkIt.Application.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddAWSLambdaHosting(LambdaEventSource.HttpApi);

    builder.Services.AddAuthorization();
    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer();

    builder.Services.ConfigureOptions<JwtBearerOptionsConfigure>();
    builder.Services.ConfigureOptions<CognitoOptionsConfigure>();

    builder.Services.AddHttpClient();

    builder.Services.AddScoped<IFlashSetService, FlashSetService>();
}

var app = builder.Build();
{
    app.UseAuthentication();
    app.UseAuthorization();

    app
        .RegisterLoginEndpoints()
        .RegisterFlashSetEndpoints()
        .RegisterFlashCardEndpoints();
}

app.Run();