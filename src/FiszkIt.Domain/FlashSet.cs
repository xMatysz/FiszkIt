using FiszkIt.Core;

namespace FiszkIt.Domain;

public class FlashSet : Entity
{
    private readonly List<FlashCard> _flashCards = new();

    public IReadOnlyCollection<FlashCard> FlashCards => _flashCards.AsReadOnly();

    public FlashSet()
        : base(Guid.NewGuid())
    {
    }
}