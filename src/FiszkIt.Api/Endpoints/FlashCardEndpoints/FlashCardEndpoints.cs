namespace FiszkIt.Api.Endpoints.FlashCardEndpoints;

public static class FlashCardEndpoints
{
    public static IEndpointRouteBuilder MapFlashCardEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGetAll()
            .MapCreate()
            .MapDelete();

        return app;
    }
}

public record CreateFlashCardRequest(Guid FlashSetId, string Question, string Answer);