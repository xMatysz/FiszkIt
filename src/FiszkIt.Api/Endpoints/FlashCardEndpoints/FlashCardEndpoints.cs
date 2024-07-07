namespace FiszkIt.Api.Endpoints.FlashCardEndpoints;

public static class FlashCardEndpoints
{
    public static IEndpointRouteBuilder RegisterFlashCardEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGetAll()
            .MapCreate()
            .MapDelete();

        return app;
    }
}