using FiszkIt.Domain.FlashCardEntity;
using FiszkIt.Domain.FlashSetEntity;

namespace FiszkIt.Tests.Shared.Builders;

public class FlashSetBuilder
{
    private Guid _creatorId = Guid.NewGuid();
    private string _name = "FlashSetTest";
    private List<FlashCard> _flashCards = [];

    public static FlashSetBuilder Default()
    {
        return new FlashSetBuilder();
    }

    public FlashSetBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    public FlashSetBuilder WithCreatorId(Guid creatorId)
    {
        _creatorId = creatorId;
        return this;
    }

    public FlashSetBuilder WithFlashCards(params FlashCard[] flashCards)
    {
        _flashCards = flashCards.ToList();
        return this;
    }

    public FlashSet Build()
    {
        var set = FlashSet.Create(_creatorId, _name).Value;

        foreach (var card in _flashCards)
        {
            set.AddFlashCard(card);
        }

        return set;
    }
}
