using FiszkIt.Application.Repository;
using FiszkIt.Domain;

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