using FiszkIt.Domain;
using FiszkIt.Infrastructure.Repository.Dtos;
using Microsoft.EntityFrameworkCore;

namespace FiszkIt.Infrastructure.Repository;

public interface IFlashSetRepository
{
    Task<FlashSetDto> AddAsync(FlashSet flashSet, CancellationToken cancellationToken);
    Task<FlashSet> GetById(Guid userId, Guid flashSetId, CancellationToken cancellationToken);
}

public class FlashSetRepository : IFlashSetRepository
{
    private readonly FiszkItDbContext _dbContext;

    public FlashSetRepository(FiszkItDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<FlashSetDto> AddAsync(FlashSet flashSet, CancellationToken cancellationToken)
    {
        await _dbContext.AddAsync(flashSet, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return new FlashSetDto(flashSet);
    }

    public Task<FlashSet> GetById(Guid userId, Guid flashSetId, CancellationToken cancellationToken)
    {
        return _dbContext.Set<FlashSet>()
            .Include(x => x.FlashCards)
            .Where(f => f.CreatorId == userId && f.Id == flashSetId)
            .FirstAsync(cancellationToken);
    }
}