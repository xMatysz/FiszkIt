using FiszkIt.Api.Common;
using FiszkIt.Application.Services;

namespace FiszkIt.Api.Endpoints.FlashSetEndpoints;

public static class Delete
{
    public static IEndpointRouteBuilder MapDelete(this IEndpointRouteBuilder app)
    {
        app.MapDelete("/flashSets/{flashSetId:guid}", async (
                Guid flashSetId,
                HttpContext context,
                IFlashSetService flashSetService,
                CancellationToken cancellationToken) =>
            {
                var result = await flashSetService.RemoveFlashSetAsync(context.GetUserId(), flashSetId, cancellationToken);

                return result.MatchFirst(
                    Results.Ok,
                    ResultsV2.Problem);
            })
            .RequireAuthorization()
            .WithName("flashSets/delete");

        return app;
    }
}
