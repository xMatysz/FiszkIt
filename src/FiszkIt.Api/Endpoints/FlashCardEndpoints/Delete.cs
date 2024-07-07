using FiszkIt.Api.Common;
using FiszkIt.Application;
using FiszkIt.Application.Repository;

namespace FiszkIt.Api.Endpoints.FlashCardEndpoints;

public static class Delete
{
    public static IEndpointRouteBuilder MapDelete(this IEndpointRouteBuilder app)
    {
        app.MapDelete("/flashCards", async (
            Guid flashSetId,
            int flashCardId,
            HttpContext context,
            IFlashSetRepository repository,
            FiszkItDbContext dbContext,
            CancellationToken cancellationToken) =>
        {
            var flashSet = await repository.GetById(context.GetUserId(), flashSetId, cancellationToken);

            flashSet.RemoveFlashCard(flashCardId);

            await dbContext.SaveChangesAsync(cancellationToken);
        });

        return app;
    }
}