using FiszkIt.Api.Common;
using FiszkIt.Infrastructure;
using FiszkIt.Infrastructure.Repository;

namespace FiszkIt.Api.Endpoints.FlashSetEndpoints;

public static class Delete
{
    public record FlashSetsDeleteRequest();
    public record FlashSetsDeleteResponse();

    public static IEndpointRouteBuilder MapDelete(this IEndpointRouteBuilder app)
    {

        app.MapDelete("/flashSets", async (
                Guid flashSetId,
                HttpContext context,
                IFlashSetRepository repository,
                FiszkItDbContext dbContext,
                CancellationToken cancellationToken) =>
            {
                var flashSet = await repository.GetById(context.GetUserId(), flashSetId, cancellationToken);

                dbContext.Remove(flashSet);
                await dbContext.SaveChangesAsync(cancellationToken);

            })
            .RequireAuthorization()
            .WithName("flashSets/delete");

        return app;
    }
}