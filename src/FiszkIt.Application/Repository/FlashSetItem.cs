using FiszkIt.Domain;

namespace FiszkIt.Application.Repository;

public class FlashSetItem
{
    public string PK { get; set; }
    public string SK { get; set; }
    public Guid Id { get; set; }
    public Guid CreatorId { get; set; }
    public string Name { get; set; }
    public List<FlashCard> FlashCards { get; set; }
    public FlashSetItem(FlashSet flashSet)
    {
        Id = flashSet.Id;
        CreatorId = flashSet.CreatorId;
        Name = flashSet.Name;
        FlashCards = new List<FlashCard>
        {
            new FlashCard("Q1", "A1"),
            new FlashCard("Q2", "A2"),
        };

        PK = $"USER#{CreatorId}";
        SK = $"SET#{Id}";
    }

    public FlashSetItem()
    {
    }
}