using FiszkIt.Api.Common;
using FiszkIt.Domain;
using FiszkIt.Application;
using FiszkIt.Application.Repository;

namespace FiszkIt.Api.Endpoints.FlashCardEndpoints;

public static class Create
{
    private record CreateFlashCardRequest(Guid FlashSetId, string Question, string Answer);

    public static IEndpointRouteBuilder MapCreate(this IEndpointRouteBuilder app)
    {
        app.MapPost("/flashCards", async (
            CreateFlashCardRequest request,
            HttpContext context,
            IFlashSetRepository repository,
            CancellationToken cancellationToken) =>
        {
            var flashSet = await repository.GetForUserById(context.GetUserId(), request.FlashSetId, cancellationToken);

            var flashCard = new FlashCard(request.Question, request.Answer);
            // flashSet.AddFlashCard(flashCard);
        });

        return app;
    }
}