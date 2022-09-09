using Todo.Domain.Events;
using Todo.Domain.Shared;

namespace Todo.Domain.Entities;

public class Todo: Entity, IAggregateRoot
{
    public string Title { get; private set; }
    public string Description { get; private set; }

    private Todo(string title, string description)
    {
        Title = title;
        Description = description;
        
        AddDomainEvents(new TodoCreatedEvent(title, description));
    }

    public static Todo Create(string title, string description)
    {
        return new Todo(title, description);
    }
}