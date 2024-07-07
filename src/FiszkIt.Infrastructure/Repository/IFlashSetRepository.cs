using FiszkIt.Domain;
using Microsoft.EntityFrameworkCore;

namespace FiszkIt.Infrastructure.Repository;

public interface IFlashSetRepository
{
    Task AddAsync(FlashSet flashSet, CancellationToken cancellationToken);
    Task<FlashSet> GetById(Guid userId, Guid flashSetId, CancellationToken cancellationToken);
}

public class FlashSetRepository : IFlashSetRepository
{
    private readonly FiszkItDbContext _dbContext;

    public FlashSetRepository(FiszkItDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(FlashSet flashSet, CancellationToken cancellationToken)
    {
        await _dbContext.AddAsync(flashSet, cancellationToken);
    }

    public Task<FlashSet> GetById(Guid userId, Guid flashSetId, CancellationToken cancellationToken)
    {
        return _dbContext.Set<FlashSet>()
            .Include(x => x.FlashCards)
            .Where(f => f.CreatorId == userId && f.Id == flashSetId)
            .FirstAsync(cancellationToken);
    }
}