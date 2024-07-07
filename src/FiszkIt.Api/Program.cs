using FiszkIt.Api.Configuration;
using FiszkIt.Api.Endpoints.FlashCardEndpoints;
using FiszkIt.Api.Endpoints.FlashSetEndpoints;
using FiszkIt.Api.Endpoints.LoginEndpoints;
using FiszkIt.Infrastructure;
using FiszkIt.Infrastructure.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;

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

    app.RegisterFlashSetEndpoints()
        .RegisterFlashCardEndpoints()
        .RegisterLoginEndpoints();

    RunMigration(app);
}

app.Run();

static void RunMigration(IHost app)
{
    using var scope = app.Services.CreateScope();
    using var dbContext = scope.ServiceProvider.GetRequiredService<FiszkItDbContext>();
    dbContext.Database.Migrate();
}