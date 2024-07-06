using FiszkIt.Api.Common;
using FiszkIt.Domain;
using FiszkIt.Infrastructure;
using FiszkIt.Infrastructure.Repository;

namespace FiszkIt.Api.Endpoints;

public static class FlashCardEndpoints
{
    public class GetFlashCardsResponse
    {
        public int Id { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }

        public GetFlashCardsResponse(FlashCard flashCard)
        {
            Id = flashCard.Id;
            Question = flashCard.Question;
            Answer = flashCard.Answer;
        }
    }
    public static IEndpointRouteBuilder MapFlashCardEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/flashCards", async (
            Guid flashSetId,
            HttpContext context,
            IFlashSetRepository repository,
            CancellationToken cancellationToken) =>
        {
            var flashSet = await repository.GetById(context.GetUserId(), flashSetId, cancellationToken);

            return flashSet.FlashCards.Select(x => new GetFlashCardsResponse(x));
        });

        app.MapPost("/flashCards", async (
            CreateFlashCardRequest request,
            HttpContext context,
            IFlashSetRepository repository,
            FiszkItDbContext dbContext,
            CancellationToken cancellationToken) =>
        {
            var flashSet = await repository.GetById(context.GetUserId(), request.FlashSetId, cancellationToken);

            var flashCard = new FlashCard(request.Question, request.Answer);
            flashSet.AddFlashCard(flashCard);

            await dbContext.SaveChangesAsync(cancellationToken);
        });

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

public record CreateFlashCardRequest(Guid FlashSetId, string Question, string Answer);