using FiszkIt.Api.Common;
using FiszkIt.Application.Repository.Dtos;
using FiszkIt.Application.Services;

namespace FiszkIt.Api.Endpoints.FlashSetEndpoints;

public static class GetAll
{
    private sealed record FlashSetsGetAllResponse(Guid Id, Guid CreatorId, string Name, FlashCardDto[] FlashCardDtos);

    public static IEndpointRouteBuilder MapGetAll(this IEndpointRouteBuilder app)
    {
        app.MapGet("/flashSets", async (
                HttpContext context,
                IFlashSetService flashSetService,
                CancellationToken cancellationToken) =>
            {
                var result = await flashSetService.GetAllForUserAsync(context.GetUserId(), cancellationToken);

                var response = result
                    .Select(f => new FlashSetsGetAllResponse(f.Id, f.CreatorId, f.Name, f.FlashCards));

                return Results.Ok(response);
            })
            .RequireAuthorization()
            .WithName("flashSets/getAll");

        return app;
    }
}
