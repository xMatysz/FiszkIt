using FiszkIt.Api.Common;
using FiszkIt.Application.Repository.Dtos;
using FiszkIt.Application.Services;

namespace FiszkIt.Api.Endpoints.FlashSetEndpoints;

public static class GetById
{
    private sealed record FlashSetsGetGetByIdResponse(Guid Id, Guid CreatorId, string Name, FlashCardDto[] FlashCardDtos);

    public static IEndpointRouteBuilder MapGetById(this IEndpointRouteBuilder app)
    {
        app.MapGet("/flashSets/{flashSetId:guid}", async (
                Guid flashSetId,
                HttpContext context,
                IFlashSetService service,
                CancellationToken cancellationToken) =>
            {
                var result = await service.GetByIdAsync(context.GetUserId(), flashSetId, cancellationToken);

                return result.MatchFirst(
                    val => Results.Ok(new FlashSetsGetGetByIdResponse(val.Id, val.CreatorId, val.Name, val.FlashCards)),
                    ResultsV2.Problem);
            })
            .RequireAuthorization()
            .WithName("flashSets/GetById");

        return app;
    }
}
