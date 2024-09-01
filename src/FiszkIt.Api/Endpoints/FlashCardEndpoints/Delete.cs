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
            Guid flashCardId,
            HttpContext context,
            IFlashSetRepository repository,
            CancellationToken cancellationToken) =>
        {
            var flashSet = await repository.GetForUserById(context.GetUserId(), flashSetId, cancellationToken);

            // flashSet.RemoveFlashCard(flashCardId);
        });

        return app;
    }
}