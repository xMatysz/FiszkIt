using FiszkIt.Api.Common;
using FiszkIt.Domain;
using FiszkIt.Infrastructure;
using FiszkIt.Infrastructure.Repository;
using Microsoft.AspNetCore.Mvc;

namespace FiszkIt.Api.Endpoints;

public static class FlashSetEndpoints
{
    public static IEndpointRouteBuilder MapFlashSetEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/flashSets", async (
            HttpContext context,
            IFlashSetRepository repository,
            CancellationToken cancellationToken) =>
        {
            var flashSets = await repository.GetAllForUser(context.GetUserId(), cancellationToken);

            return flashSets.Select(f=>new GetFlashSetResponse(f));
        }).RequireAuthorization();

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

        app.MapDelete("/flashSets", async (
            Guid flashSetId,
            HttpContext context,
            IFlashSetRepository repository,
            FiszkItDbContext dbContext,
            CancellationToken cancellationToken) =>
        {
            var flashSet = await repository.GetById(context.GetUserId(), flashSetId, cancellationToken);

            dbContext.Remove(flashSet);
            await dbContext.SaveChangesAsync(cancellationToken);

        }).RequireAuthorization();

        return app;
    }
}