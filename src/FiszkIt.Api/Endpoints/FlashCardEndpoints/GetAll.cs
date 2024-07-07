using FiszkIt.Api.Common;
using FiszkIt.Infrastructure.Repository;

namespace FiszkIt.Api.Endpoints.FlashCardEndpoints;

public static class GetAll
{
    public record FlashCartGetAllResponse(int Id, string Question, string Answer);

    public static IEndpointRouteBuilder MapGetAll(this IEndpointRouteBuilder app)
    {
        app.MapGet("/flashCards", async (
            Guid flashSetId,
            HttpContext context,
            IFlashSetRepository repository,
            CancellationToken cancellationToken) =>
        {
            var flashSet = await repository.GetById(context.GetUserId(), flashSetId, cancellationToken);

            return flashSet.FlashCards.Select(x => new FlashCartGetAllResponse(x.Id, x.Question, x.Answer));
        });

        return app;
    }
}