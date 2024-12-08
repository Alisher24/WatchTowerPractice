namespace WatchTower.Domain.Shared;

public abstract class Entity<TId>(TId id)
    where TId : notnull
{
    public TId Id { get; private set; } = id;
}