using FiszkIt.Domain;

namespace FiszkIt.Application.Repository.Dtos;

public class FlashCardDto
{
    public Guid Id { get; set; }
    public string Question { get; set; }
    public string Answer { get; set; }

    public FlashCardDto(FlashCard card)
    {
        Id = card.Id;
        Question = card.Question;
        Answer = card.Answer;
    }
}