using FiszkIt.Application.Repository;
using FiszkIt.Application.Repository.Dtos;

namespace FiszkIt.Application.Services;

public interface IFlashSetService
{
    Task<FlashSetDto[]> GetAllAsync(Guid userId, CancellationToken cancellationToken);
}

public class FlashSetService : IFlashSetService
{
    private readonly IFlashSetDtoRepository _flashSetDtoRepository;

    public FlashSetService(IFlashSetDtoRepository flashSetDtoRepository)
    {
        _flashSetDtoRepository = flashSetDtoRepository;
    }

    public Task<FlashSetDto[]> GetAllAsync(Guid userId, CancellationToken cancellationToken)
    {
        return _flashSetDtoRepository.GetAllForUserAsync(userId, cancellationToken);
    }
}