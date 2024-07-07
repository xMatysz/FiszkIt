namespace FiszkIt.Api.Endpoints.FlashSetEndpoints;

public static class FlashSetEndpoints
{
    public static IEndpointRouteBuilder MapFlashSetEndpoints(this IEndpointRouteBuilder app)
    {
        app
            .MapGetAll()
            .MapCreate()
            .MapDelete();

        return app;
    }
}