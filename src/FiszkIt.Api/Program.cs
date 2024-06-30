using FiszkIt.Domain;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddAWSLambdaHosting(LambdaEventSource.HttpApi);
}

var app = builder.Build();
{
    app.MapGet("/", () =>
    {
        var x = new Flashcard("HowAreYou?", "Good");
        return x;
    });

}

app.Run();
