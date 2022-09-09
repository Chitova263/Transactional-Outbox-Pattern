using Todo.Shared.Events;

namespace Todo.Shared.Rabbitmq;

public interface IEventPublisher
{
    Task PublishAsync<TEvent>(string exchange, string routingKey, TEvent message, string? messageId = default) where TEvent : class, IEvent;
    Task PublishAsync(string exchange, string routingKey, string message, string? messageId = default);
}