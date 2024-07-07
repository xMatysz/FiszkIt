using FiszkIt.Api.Common;
using FiszkIt.Infrastructure.Repository;
using FiszkIt.Infrastructure.Repository.Dtos;

namespace FiszkIt.Api.Endpoints.FlashSetEndpoints;

public static class GetAll
{
    private record FlashSetsGetAllResponse(Guid Id, Guid CreatorId, string Name, FlashCardDto[] FlashCardDtos);

    public static IEndpointRouteBuilder MapGetAll(this IEndpointRouteBuilder app)
    {
        app.MapGet("/flashSets", async (
            HttpContext context,
            IFlashSetDtoRepository repository,
            CancellationToken cancellationToken) =>
        {
            var flashSets = await repository.GetAllForUser(context.GetUserId(), cancellationToken);
            var response = flashSets
                .Select(f => new FlashSetsGetAllResponse(f.Id, f.CreatorId, f.Name, f.FlashCards));

            return Results.Ok(response);
        }).RequireAuthorization();

        return app;
    }
}