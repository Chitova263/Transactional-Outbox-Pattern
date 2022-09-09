using Todo.Domain.Shared;

namespace Todo.Domain.Events;

public class TodoCreatedEvent : IIntegrationEvent
{
    public Guid Id { get; }
    public string Title { get; }
    public string Description { get; }

    public TodoCreatedEvent(string title, string description)
    {
        Id = Guid.NewGuid();
        Title = title;
        Description = description;
    }
}