using FiszkIt.Domain.FlashCardEntity;

namespace FiszkIt.Application.Repository.Dtos;

public record FlashCardDto(Guid Id, string Question, string Answer)
{
    public FlashCardDto(FlashCard card)
        : this(card.Id, card.Question, card.Answer)
    {
    }

    public FlashCardDto()
        : this(Guid.Empty, string.Empty, string.Empty)
    {
    }

    public FlashCard ToEntity()
    {
        return FlashCard.CreatePerfect(Id, Question, Answer);
    }
}