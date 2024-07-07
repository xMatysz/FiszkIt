using FiszkIt.Domain;
using FiszkIt.Infrastructure.Repository.Dtos;
using Microsoft.EntityFrameworkCore;

namespace FiszkIt.Infrastructure.Repository;

public interface IFlashSetDtoRepository
{
    Task<FlashSetDto[]> GetAllForUser(Guid userId, CancellationToken cancellationToken);
}

public class FlashSetDtoRepository : IFlashSetDtoRepository
{
    private readonly FiszkItDbContext _dbContext;

    public FlashSetDtoRepository(FiszkItDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<FlashSetDto[]> GetAllForUser(Guid userId, CancellationToken cancellationToken)
    {
        return _dbContext.Set<FlashSet>()
            .AsNoTracking()
            .Where(f => f.CreatorId == userId)
            .Select(x => new FlashSetDto(x))
            .ToArrayAsync(cancellationToken);
    }
}