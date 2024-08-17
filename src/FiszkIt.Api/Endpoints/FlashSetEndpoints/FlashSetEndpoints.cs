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

        app.MapPost("/add", async () =>
        {
            var repo = new FlashSetDynamoDbRepository();
            await repo.AddAsync(FlashSet.Create(Guid.NewGuid(), "TEST").Value, default);
        });
        
        app.MapPost("/get", async (Guid user, Guid set) =>
        {
            var repo = new FlashSetDynamoDbRepository();
            await repo.GetById(user, set, default);
        });

        return app;
    }
}