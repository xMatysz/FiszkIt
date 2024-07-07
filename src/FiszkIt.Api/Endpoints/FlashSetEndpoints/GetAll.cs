using FiszkIt.Api.Common;
using FiszkIt.Infrastructure.Repository;

namespace FiszkIt.Api.Endpoints.FlashSetEndpoints;

public static class GetAll
{
    public record FlashSetsGetAllRequest();
    public record FlashSetsGetAllResponse();
    public static IEndpointRouteBuilder MapGetAll(this IEndpointRouteBuilder app)
    {
        app.MapGet("/flashSets", async (
            HttpContext context,
            IFlashSetRepository repository,
            CancellationToken cancellationToken) =>
        {
            var flashSets = await repository.GetAllForUser(context.GetUserId(), cancellationToken);

            return flashSets.Select(f=>new GetFlashSetResponse(f));
        }).RequireAuthorization();

        return app;
    }
}