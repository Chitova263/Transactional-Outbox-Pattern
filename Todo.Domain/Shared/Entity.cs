namespace Todo.Domain.Shared;

public abstract class Entity    
{
    public Guid Id { get; protected set; }
    private readonly List<IDomainEvent> _domainEvents = new List<IDomainEvent>();
    public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    protected void AddDomainEvents(IDomainEvent @event)
    {
        _domainEvents.Add(@event);
    }

    public void RemoveDomainEvent(IDomainEvent @event)
    {
        _domainEvents.Remove(@event);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}