using FiszkIt.Application.Repository;
using FiszkIt.Application.Repository.Dtos;

public sealed class GetFlashSetResponse
{
    public Guid Id { get; set; }
    public Guid CreatorId { get; set; }
    public string Name { get; set; }

    public GetFlashSetResponse(FlashSetDto dto)
    {
        Id = dto.Id;
        CreatorId = dto.CreatorId;
        Name = dto.Name;
    }
}