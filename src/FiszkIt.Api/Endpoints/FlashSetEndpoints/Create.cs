using FiszkIt.Api.Common;
using FiszkIt.Domain;
using FiszkIt.Infrastructure;
using FiszkIt.Infrastructure.Repository;
using Microsoft.AspNetCore.Mvc;

namespace FiszkIt.Api.Endpoints.FlashSetEndpoints;

public static class Create
{
    public record FlashSetsCreateRequest();
    public record FlashSetsCreateResponse();
    public static IEndpointRouteBuilder MapCreate(this IEndpointRouteBuilder app)
    {
        app.MapPost("/flashSets", async (
            [FromBody] CreateFlashSetRequest request,
            HttpContext context,
            IFlashSetRepository repository,
            FiszkItDbContext dbContext,
            CancellationToken cancellationToken) =>
        {
            var flashSet = FlashSet.Create(context.GetUserId(), request.Name);

            await repository.AddAsync(flashSet.Value, cancellationToken);

            await dbContext.SaveChangesAsync(cancellationToken);
        }).RequireAuthorization();

        return app;
    }
}