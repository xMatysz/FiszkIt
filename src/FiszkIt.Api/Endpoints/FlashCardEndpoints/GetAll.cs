using FiszkIt.Api.Common;
using FiszkIt.Application.Repository;

namespace FiszkIt.Api.Endpoints.FlashCardEndpoints;

public static class GetAll
{
    public record FlashCartGetAllResponse(Guid Id, string Question, string Answer);

    public static IEndpointRouteBuilder MapGetAll(this IEndpointRouteBuilder app)
    {
        app.MapGet("/flashCards", async (
            Guid flashSetId,
            HttpContext context,
            IFlashSetRepository repository,
            CancellationToken cancellationToken) =>
        {
            var flashSet = await repository.GetForUserById(context.GetUserId(), flashSetId, cancellationToken);

            if (flashSet is null)
            {
                return Results.NotFound("WTf");
            }

            var response = flashSet.FlashCards
                .Select(x => new FlashCartGetAllResponse(x.Id, x.Question, x.Answer));
            
            return Results.Ok(response);
        });

        return app;
    }
}