namespace CleanArchitecture.Core.Events;

public abstract class EventBase
{
    protected EventBase()
    {
        OccuredAt = DateTimeOffset.Now;
        Id = Guid.NewGuid();
    }

    public Guid Id { get; }
    public DateTimeOffset OccuredAt { get; }
}