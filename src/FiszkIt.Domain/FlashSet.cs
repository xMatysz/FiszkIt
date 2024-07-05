using FiszkIt.Core;

namespace FiszkIt.Domain;

public class FlashSet : Entity
{
    private readonly List<FlashCard> _flashCards = new();
    public IReadOnlyCollection<FlashCard> FlashCards => _flashCards.AsReadOnly();

    public Guid CreatorId { get; set; }

    public FlashSet(Guid userId)
        : base(Guid.NewGuid())
    {
        CreatorId = userId;
    }

    public void AddFlashCard(FlashCard flashCard)
    {
        _flashCards.Add(flashCard);
    }

    public void RemoveFlashCard(FlashCard flashCard)
    {
        _flashCards.Remove(flashCard);
    }

    private FlashSet()
    {
    }
}