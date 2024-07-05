namespace FiszkIt.Infrastructure.Repository;

public interface IFlashSetRepository
{
    Task<FlashSetDto[]> GetAllForUser(Guid userId, CancellationToken cancellationToken);
}

public class FlashSetDynamoRepository : IFlashSetRepository
{
    public Task<FlashSetDto[]> GetAllForUser(Guid userId, CancellationToken cancellationToken)
    {
        return Task.FromResult((FlashSetDto[])[new FlashSetDto()]);
    }
}

public class FlashSetDto
{
}