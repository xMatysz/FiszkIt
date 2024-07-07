using FiszkIt.Api.Common;
using FiszkIt.Application;
using FiszkIt.Application.Repository;

namespace FiszkIt.Api.Endpoints.FlashSetEndpoints;

public static class Delete
{
    public static IEndpointRouteBuilder MapDelete(this IEndpointRouteBuilder app)
    {

        app.MapDelete("/flashSets/{flashSetId:guid}", async (
                Guid flashSetId,
                HttpContext context,
                IFlashSetRepository repository,
                FiszkItDbContext dbContext,
                CancellationToken cancellationToken) =>
            {
                var flashSet = await repository.GetById(context.GetUserId(), flashSetId, cancellationToken);

                dbContext.Remove(flashSet);
                await dbContext.SaveChangesAsync(cancellationToken);

                return Results.NoContent();
            })
            .RequireAuthorization()
            .WithName("flashSets/delete");

        return app;
    }
}