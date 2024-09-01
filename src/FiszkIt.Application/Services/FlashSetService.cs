using FiszkIt.Application.Repository;
using FiszkIt.Application.Repository.Dtos;
using ErrorOr;
using FiszkIt.Domain;

namespace FiszkIt.Application.Services;

public interface IFlashSetService
{
    Task<FlashSetDto[]> GetAllForUserAsync(Guid userId, CancellationToken cancellationToken);
    Task<ErrorOr<FlashSetDto>> GetByIdAsync(Guid flashSetId, Guid userId, CancellationToken cancellationToken);
    Task<ErrorOr<FlashSetDto>> CreateFlashSet(Guid userId, string flashSetName, CancellationToken cancellationToken);
    Task<ErrorOr<Deleted>> RemoveFlashSetAsync(Guid userId, Guid flashSetId, CancellationToken cancellationToken);
}

public class FlashSetService : IFlashSetService
{
    private readonly IFlashSetRepository _flashSetRepository;

    public FlashSetService(IFlashSetRepository flashSetRepository)
    {
        _flashSetRepository = flashSetRepository;
    }

    public Task<FlashSetDto[]> GetAllForUserAsync(Guid userId, CancellationToken cancellationToken)
    {
        return _flashSetRepository.GetAllForUser(userId, cancellationToken);
    }

    public async Task<ErrorOr<FlashSetDto>> GetByIdAsync(Guid flashSetId, Guid userId, CancellationToken cancellationToken)
    {
        var flashSet = await _flashSetRepository.GetForUserById(flashSetId, userId, cancellationToken);
        return flashSet is not null ? flashSet : FlashSetErrors.NotFound;
    }

    public async Task<ErrorOr<FlashSetDto>> CreateFlashSet(Guid userId, string flashSetName, CancellationToken cancellationToken)
    {
        var flashSet = FlashSet.Create(userId, flashSetName);
        if (flashSet.IsError)
        {
            return flashSet.Errors;
        }

        return await _flashSetRepository.AddAsync(flashSet.Value, cancellationToken);
    }

    public async Task<ErrorOr<Deleted>> RemoveFlashSetAsync(Guid userId, Guid flashSetId, CancellationToken cancellationToken)
    {
        var isDeleted = await _flashSetRepository.RemoveAsync(userId, flashSetId, cancellationToken);

        return isDeleted
            ? Result.Deleted
            : Error.NotFound();
    }
}