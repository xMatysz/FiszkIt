using FiszkIt.Domain;
using Microsoft.EntityFrameworkCore;

namespace FiszkIt.Infrastructure.Repository;

public interface IFlashSetRepository
{
    Task<FlashSetDto[]> GetAllForUser(Guid userId, CancellationToken cancellationToken);

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

    public Task<FlashSetDto[]> GetAllForUser(Guid userId, CancellationToken cancellationToken)
    {
        return _dbContext.Set<FlashSet>()
            .AsNoTracking()
            .Where(f => f.CreatorId == userId)
            .Select(x => new FlashSetDto(x))
            .ToArrayAsync(cancellationToken);
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

public class FlashSetDto
{
    public Guid Id { get; set; }
    public Guid CreatorId { get; set; }
    public string Name { get; set; }

    public FlashSetDto(FlashSet set)
    {
        Id = set.Id;
        CreatorId = set.CreatorId;
        Name = set.Name;
    }
}