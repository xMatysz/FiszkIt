namespace FiszkIt.Api.Endpoints.FlashSetEndpoints;

public static class FlashSetEndpoints
{
    public static IEndpointRouteBuilder RegisterFlashSetEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGetAll()
            .MapGetById()
            .MapCreate()
            .MapDelete();

        return app;
    }
}