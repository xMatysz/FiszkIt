using FiszkIt.Api.Common;
using FiszkIt.Application.Repository.Dtos;
using FiszkIt.Application.Services;

namespace FiszkIt.Api.Endpoints.FlashSetEndpoints;

public static class Create
{
    private record FlashSetsCreateRequest(string Name);
    private record FlashSetsCreateResponse(Guid Id, Guid CreatorId, string Name, FlashCardDto[] FlashCardDtos);
    public static IEndpointRouteBuilder MapCreate(this IEndpointRouteBuilder app)
    {
        app.MapPost("/flashSets", async (
                FlashSetsCreateRequest request,
                HttpContext context,
                IFlashSetService flashSetService,
                CancellationToken cancellationToken) =>
            {
                var result = await flashSetService.CreateFlashSet(context.GetUserId(), request.Name, cancellationToken);

                if (result.IsError)
                {
                    ResultsV2.Problem(result.Errors);
                }

                var set = result.Value;
                var response = new FlashSetsCreateResponse(set.Id, set.CreatorId, set.Name, set.FlashCards);

                return Results.CreatedAtRoute(
                    "FlashSets/GetById",
                    new { flashSetId = response.Id },
                    response);
            })
            .RequireAuthorization()
            .WithName("flashSets/create");

        return app;
    }
}