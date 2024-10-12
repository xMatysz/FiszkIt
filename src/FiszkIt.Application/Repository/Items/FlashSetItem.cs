using FiszkIt.Domain.FlashCardEntity;
using FiszkIt.Domain.FlashSetEntity;

namespace FiszkIt.Application.Repository.Items;

public record FlashSetItem(string PK, string SK, Guid Id, Guid CreatorId, string Name, FlashCard[] FlashCards)
{
    public FlashSetItem(Guid userId, FlashSet flashSet)
        : this(CreatePk(userId), CreateSk(flashSet), flashSet.Id, flashSet.CreatorId, flashSet.Name, flashSet.FlashCards.ToArray())
    {
    }

    public static string CreatePk(Guid userId)
        => $"USER#{userId.ToString()}";

    public static string CreateSk(Guid flashSetId)
        => $"FLASHSET#{flashSetId.ToString()}";

    private static string CreatePk(FlashSet flashSet)
        => CreatePk(flashSet.CreatorId);

    private static string CreateSk(FlashSet flashSet)
        => CreateSk(flashSet.Id);

    // required for deserialization
    public FlashSetItem()
        : this(string.Empty, string.Empty, Guid.Empty, Guid.Empty, string.Empty, [])
    {
    }
}