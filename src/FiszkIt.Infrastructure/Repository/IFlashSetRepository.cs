using FiszkIt.Domain;
using Microsoft.EntityFrameworkCore;

namespace FiszkIt.Infrastructure.Repository;

public interface IFlashSetRepository
{
    Task<FlashSetDto[]> GetAllForUser(Guid userId, CancellationToken cancellationToken);

    Task AddAsync(FlashSet flashSet, CancellationToken cancellationToken);
}

public class FlashSetRepository : IFlashSetRepository
{
    private readonly FiszkItDbContext _dbContext;

    public FlashSetRepository(FiszkItDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<FlashSetDto[]> GetAllForUser(Guid userId, CancellationToken cancellationToken)
    {
        return await _dbContext.Set<FlashSet>()
            .AsNoTracking()
            .Where(f => f.CreatorId == userId)
            .Select(x => new FlashSetDto(x))
            .ToArrayAsync(cancellationToken);
    }

    public async Task AddAsync(FlashSet flashSet, CancellationToken cancellationToken)
    {
        await _dbContext.AddAsync(flashSet, cancellationToken);
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