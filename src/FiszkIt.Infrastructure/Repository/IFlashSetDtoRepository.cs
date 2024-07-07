using FiszkIt.Domain;
using FiszkIt.Infrastructure.Repository.Dtos;
using Microsoft.EntityFrameworkCore;

namespace FiszkIt.Infrastructure.Repository;

public interface IFlashSetDtoRepository
{
    Task<FlashSetDto?> GetByIdForUser(Guid flashCardId, Guid userId, CancellationToken cancellationToken);
    Task<FlashSetDto[]> GetAllForUser(Guid userId, CancellationToken cancellationToken);
}

public class FlashSetDtoRepository : IFlashSetDtoRepository
{
    private readonly FiszkItDbContext _dbContext;

    public FlashSetDtoRepository(FiszkItDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<FlashSetDto?> GetByIdForUser(Guid flashCardId, Guid userId, CancellationToken cancellationToken)
    {
        var flashCard = await _dbContext.Set<FlashSet>()
            .Include(x => x.FlashCards)
            .AsNoTracking()
            .FirstOrDefaultAsync(f => f.Id == flashCardId && f.CreatorId == userId, cancellationToken);

        return flashCard is null ? null : new FlashSetDto(flashCard);
    }

    public Task<FlashSetDto[]> GetAllForUser(Guid userId, CancellationToken cancellationToken)
    {
        return _dbContext.Set<FlashSet>()
            .Include(x => x.FlashCards)
            .AsNoTracking()
            .Where(f => f.CreatorId == userId)
            .Select(x => new FlashSetDto(x))
            .ToArrayAsync(cancellationToken);
    }
}