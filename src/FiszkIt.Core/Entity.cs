namespace FiszkIt.Core;

public abstract class Entity
{
    public Guid Id { get; protected init; }

    protected Entity(Guid id)
    {
        Id = id;
    }

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

    private Entity(){}
}