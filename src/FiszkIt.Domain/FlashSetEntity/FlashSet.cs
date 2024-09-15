using ErrorOr;
using FiszkIt.Core;
using FiszkIt.Core.Extensions;
using FiszkIt.Domain.FlashCardEntity;

namespace FiszkIt.Domain.FlashSetEntity;

public class FlashSet : Entity
{
    private readonly List<FlashCard> _flashCards = new();

    public IReadOnlyCollection<FlashCard> FlashCards => _flashCards.AsReadOnly();
    public Guid CreatorId { get; init; }
    public string Name { get; init; }

    private FlashSet(Guid userId, string name, Guid? id = null)
        : base(id ?? Guid.NewGuid())
    {
        CreatorId = userId;
        Name = name;
    }

    public static ErrorOr<FlashSet> Create(Guid creatorId, string name)
    {
        if (name.IsNullOrEmpty())
        {
            return FlashSetErrors.NameCannotBeEmpty;
        }

        return new FlashSet(creatorId, name);
    }

    public void AddFlashCard(FlashCard flashCard)
    {
        _flashCards.Add(flashCard);
    }

    public void RemoveFlashCard(Guid flashCardId)
    {
        _flashCards.RemoveAll(f => f.Id == flashCardId);
    }

    public static FlashSet CreatePerfect(Guid id, string name, Guid creatorId, IEnumerable<FlashCard> flashCards)
    {
        var set = new FlashSet(creatorId, name, id);
        foreach (var flashCard in flashCards)
        {
            set.AddFlashCard(flashCard);
        }

        return set;
    }
}