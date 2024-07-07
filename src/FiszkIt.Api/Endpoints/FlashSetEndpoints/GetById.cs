using FiszkIt.Api.Common;
using FiszkIt.Infrastructure.Repository;
using FiszkIt.Infrastructure.Repository.Dtos;

namespace FiszkIt.Api.Endpoints.FlashSetEndpoints;

public static class GetById
{
    private record FlashSetsGetGetByIdResponse(Guid Id, Guid CreatorId, string Name, FlashCardDto[] FlashCardDtos);

    public static IEndpointRouteBuilder MapGetById(this IEndpointRouteBuilder app)
    {
        app.MapGet("/flashSets/{flashSetId:guid}", async (
                Guid flashSetId,
                HttpContext context,
                IFlashSetDtoRepository repository,
                CancellationToken cancellationToken) =>
            {
                var flashSet = await repository.GetByIdForUserAsync(flashSetId, context.GetUserId(), cancellationToken);

                if (flashSet is null)
                {
                    return Results.NotFound();
                }

                var response = new FlashSetsGetGetByIdResponse(flashSet.Id, flashSet.CreatorId, flashSet.Name,
                    flashSet.FlashCards);

                return Results.Ok(response);
            })
            .RequireAuthorization()
            .WithName("flashSets/GetById");

        return app;
    }
}