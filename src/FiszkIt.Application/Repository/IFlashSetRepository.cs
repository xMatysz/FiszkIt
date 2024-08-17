using FiszkIt.Domain;
using FiszkIt.Application.Repository.Dtos;

namespace FiszkIt.Application.Repository;

public interface IFlashSetRepository
{
    Task<FlashSetDto> AddAsync(FlashSet flashSet, CancellationToken cancellationToken);
    Task<FlashSet> GetById(Guid userId, Guid flashSetId, CancellationToken cancellationToken);
}