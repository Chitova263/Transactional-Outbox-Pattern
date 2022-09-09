using MediatR;
using Todo.Shared.Events;

namespace Todo.Feed.Events;

public class TodoAddedEvent : IEvent
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
}