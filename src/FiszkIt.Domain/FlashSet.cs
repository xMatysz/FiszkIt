using ErrorOr;
using FiszkIt.Core;
using FiszkIt.Core.Extensions;

namespace FiszkIt.Domain;

public class FlashSet : Entity
{
    private readonly List<FlashCard> _flashCards = new();
    public IReadOnlyCollection<FlashCard> FlashCards => _flashCards.AsReadOnly();

    public Guid CreatorId { get; init; }
    public string Name { get; init; }

    private FlashSet(Guid userId, string name)
        : base(Guid.NewGuid())
    {
        CreatorId = userId;
        Name = name;
    }

    public static ErrorOr<FlashSet> Create(Guid creatorId, string name)
    {
        if (name.IsNullOrEmpty())
        {
            return Error.Validation();
        }

        return new FlashSet(creatorId, name);
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