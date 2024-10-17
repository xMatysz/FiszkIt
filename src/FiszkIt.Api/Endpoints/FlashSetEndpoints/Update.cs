using FiszkIt.Api.Common;
using FiszkIt.Application.Repository.Dtos;
using FiszkIt.Application.Services;

namespace FiszkIt.Api.Endpoints.FlashSetEndpoints;

public static class Update
{
    private sealed record FlashSetUpdateRequest(FlashSetDto FlashSetDto);

    public static IEndpointRouteBuilder MapUpdate(this IEndpointRouteBuilder app)
    {
        app.MapPut("/flashsets", async (
                FlashSetUpdateRequest request,
                HttpContext context,
                IFlashSetService flashSetService,
                CancellationToken cancellationToken) =>
            {
                var result = await flashSetService
                    .UpdateFlashSetAsync(context.GetUserId(), request.FlashSetDto, cancellationToken);

                if (result.IsError)
                {
                    ResultsV2.Problem(result.Errors);
                }

                return Results.Ok(result.Value);
            })
            .RequireAuthorization()
            .WithName("flashSets/update");

        return app;
    }
}
