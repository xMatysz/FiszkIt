namespace FiszkIt.Core;

public abstract class Entity(Guid id)
{
    public Guid Id { get; init; } = id;

    public override bool Equals(object? obj)
    {
        if (obj is null || obj.GetType() != GetType())
        {
            return false;
        }

        return ((Entity)obj).Id == Id;
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    protected Entity()
        : this(Guid.Empty)
    {
    }
}