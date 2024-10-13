using FiszkIt.Application.Repository.Items;
using FiszkIt.Domain.FlashCardEntity;
using FiszkIt.Domain.FlashSetEntity;

namespace FiszkIt.Application.Repository.Dtos;

public record FlashSetDto(Guid Id, Guid CreatorId, string Name, FlashCardDto[] FlashCards)
{
    public FlashSetDto(FlashSetItem set)
        : this(set.Id, set.CreatorId, set.Name, ConvertToDtos(set.FlashCards))
    {
    }

    private static FlashCardDto[] ConvertToDtos(IEnumerable<FlashCard> setFlashCards)
        => setFlashCards.Select(x => new FlashCardDto(x)).ToArray();

    public FlashSet ToEntity()
    {
        var cards = FlashCards.Select(x => x.ToEntity());
        return FlashSet.CreatePerfect(Id, Name, CreatorId, cards);
    }
}
