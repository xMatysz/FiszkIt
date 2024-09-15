using FiszkIt.Application.Repository;
using FiszkIt.Application.Repository.Dtos;
using ErrorOr;
using FiszkIt.Domain.FlashSetEntity;

namespace FiszkIt.Application.Services;

public interface IFlashSetService
{
    Task<FlashSetDto[]> GetAllForUserAsync(Guid userId, CancellationToken cancellationToken);
    Task<ErrorOr<FlashSetDto>> GetByIdAsync(Guid userId, Guid flashSetId, CancellationToken cancellationToken);
    Task<ErrorOr<FlashSetDto>> CreateFlashSet(Guid userId, string flashSetName, CancellationToken cancellationToken);
    Task<ErrorOr<Deleted>> RemoveFlashSetAsync(Guid userId, Guid flashSetId, CancellationToken cancellationToken);
    Task<ErrorOr<Updated>> UpdateFlashSetAsync(Guid userId, FlashSetDto flashSetDto, CancellationToken cancellationToken);
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

    public async Task<ErrorOr<FlashSetDto>> GetByIdAsync(Guid userId, Guid flashSetId, CancellationToken cancellationToken)
    {
        return await _flashSetRepository.GetForUserById(userId, flashSetId, cancellationToken);
    }

    public async Task<ErrorOr<FlashSetDto>> CreateFlashSet(Guid userId, string flashSetName, CancellationToken cancellationToken)
    {
        var flashSet = FlashSet.Create(userId, flashSetName);

        if (flashSet.IsError)
        {
            return flashSet.Errors;
        }

        return await _flashSetRepository.AddAsync(userId, flashSet.Value, cancellationToken);
    }

    public async Task<ErrorOr<Deleted>> RemoveFlashSetAsync(Guid userId, Guid flashSetId, CancellationToken cancellationToken)
    {
        return await _flashSetRepository.RemoveAsync(userId, flashSetId, cancellationToken);
    }

    public Task<ErrorOr<Updated>> UpdateFlashSetAsync(Guid userId, FlashSetDto flashSetDto, CancellationToken cancellationToken)
    {
        var flashSet = flashSetDto.ToEntity();
        return _flashSetRepository.UpdateAsync(userId, flashSet, cancellationToken);
    }
}