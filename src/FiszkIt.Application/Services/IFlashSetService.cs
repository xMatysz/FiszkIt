using FiszkIt.Application.Repository;
using FiszkIt.Application.Repository.Dtos;
using ErrorOr;
using FiszkIt.Domain;

namespace FiszkIt.Application.Services;

public interface IFlashSetService
{
    Task<FlashSetDto[]> GetAllAsync(Guid userId, CancellationToken cancellationToken);
    Task<ErrorOr<FlashSetDto>> GetByIdAsync(Guid flashSetId, Guid userId, CancellationToken cancellationToken);
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

    public async Task<ErrorOr<FlashSetDto>> GetByIdAsync(Guid flashSetId, Guid userId, CancellationToken cancellationToken)
    {
        var flashSet = await _flashSetDtoRepository.GetByIdForUserAsync(flashSetId, userId, cancellationToken);
        return flashSet is not null ? flashSet : FlashSetErrors.NotFound;
    }
}