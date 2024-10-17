namespace FiszkIt.Api.Endpoints.FlashSetEndpoints;

public static class FlashSetEndpointsExtension
{
    public static IEndpointRouteBuilder RegisterFlashSetEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGetAll()
            .MapGetById()
            .MapCreate()
            .MapDelete()
            .MapUpdate();

        return app;
    }
}
