using RabbitMQ.Client.Events;
using Todo.Shared.Events;

namespace Todo.Shared.Rabbitmq;

public interface IEventSubscriber
{
    IEventSubscriber Subscribe<TEvent>(string queue, string routingKey, string exchange,
        Func<TEvent, BasicDeliverEventArgs, Task> handle) where TEvent: class, IEvent;
}
