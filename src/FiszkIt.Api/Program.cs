using FiszkIt.Domain;
using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () =>
{
    var x = new Flashcard("HowAreYou?", "Good");
    return x;
});

app.Run();
