using FiszkIt.Api.Common;
using FiszkIt.Domain;
using FiszkIt.Infrastructure.Repository;
using FiszkIt.Infrastructure.Repository.Dtos;

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
                IFlashSetRepository repository,
                CancellationToken cancellationToken) =>
            {
                var flashSet = FlashSet.Create(context.GetUserId(), request.Name);

                var set = await repository.AddAsync(flashSet.Value, cancellationToken);
                var response = new FlashSetsCreateResponse(set.Id, set.CreatorId, set.Name, set.FlashCards);

                return Results.CreatedAtRoute(
                    "FlashSets/GetById", new { flashSetId = response.Id }, response);

            })
            .RequireAuthorization()
            .WithName("flashSets/create");

        return app;
    }
}