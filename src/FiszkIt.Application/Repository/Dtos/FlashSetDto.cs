using FiszkIt.Domain;

namespace FiszkIt.Application.Repository.Dtos;

public class FlashSetDto
{
    public Guid Id { get; set; }
    public Guid CreatorId { get; set; }
    public string Name { get; set; }
    public FlashCardDto[] FlashCards { get; set; }

    public FlashSetDto(FlashSet set)
    {
        Id = set.Id;
        CreatorId = set.CreatorId;
        Name = set.Name;
        FlashCards = set.FlashCards.Select(f=>new FlashCardDto(f)).ToArray();
    }
}