using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using Todo.Shared.Events;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Todo.Shared.Rabbitmq;

public class EventPublisher : IEventPublisher
{
    private readonly ConnectionFactory _connectionFactory;

    public EventPublisher()
    {
        _connectionFactory = new ConnectionFactory
        {
            Uri = new Uri("amqps://wtjodomp:0OE0eikprg_-bStY5dDsl_a3O1MokOQQ@chimpanzee.rmq.cloudamqp.com/wtjodomp")
        };
    }
    
    public Task PublishAsync<TEvent>(
        string exchange, 
        string routingKey, 
        TEvent @event, 
        string? messageId = default) where TEvent : class, IEvent
    {
        var connection = _connectionFactory.CreateConnection();
        var channel = connection.CreateModel();
     
        var props = channel.CreateBasicProperties();
        props.DeliveryMode = 2;
        props.MessageId = messageId ?? Guid.NewGuid().ToString("N");
        props.Timestamp = new AmqpTimestamp(DateTime.UtcNow.Ticks);
        
        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(@event));
        
        channel.BasicPublish(
            exchange: exchange,
            routingKey: routingKey,
            basicProperties: props,
            mandatory: true,
            body: body
        );
        return Task.CompletedTask;
    }

    public Task PublishAsync(string exchange, string routingKey, string message, string? messageId = default)
    {
        var connection = _connectionFactory.CreateConnection();
        var channel = connection.CreateModel();
     
        var props = channel.CreateBasicProperties();
        props.DeliveryMode = 2;
        props.MessageId = messageId ?? Guid.NewGuid().ToString("N");
        props.Timestamp = new AmqpTimestamp(DateTime.UtcNow.Ticks);
        var json = JsonConvert.DeserializeObject(message);
        var body = Encoding.UTF8.GetBytes(json.ToString());
        channel.BasicPublish(
            exchange: exchange,
            routingKey: routingKey,
            basicProperties: props,
            mandatory: true,
            body: body
        );
        return Task.CompletedTask;
    }
}