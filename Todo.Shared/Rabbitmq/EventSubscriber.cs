using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Todo.Shared.Events;

namespace Todo.Shared.Rabbitmq;

public class EventSubscriber : IEventSubscriber
{
    private readonly IModel _channel;

    public EventSubscriber()
    {
        var connectionFactory = new ConnectionFactory
        {
            Uri = new Uri("amqps://wtjodomp:0OE0eikprg_-bStY5dDsl_a3O1MokOQQ@chimpanzee.rmq.cloudamqp.com/wtjodomp")
        };
        var connection = connectionFactory.CreateConnection();
        _channel = connection.CreateModel();
    }
    
    public IEventSubscriber Subscribe<TEvent>(string queue, string routingKey, string exchange, Func<TEvent, BasicDeliverEventArgs, Task> handle)
        where TEvent: class, IEvent
    {
        _channel.ExchangeDeclare(exchange, "topic", durable: true, autoDelete: false, null);
        _channel.QueueDeclare(queue, durable: true, autoDelete: false, exclusive: false);
        _channel.QueueBind(queue, exchange, routingKey);
        
        _channel.BasicQos(0, 1, false);

        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var json = Encoding.UTF8.GetString(body);
            var message = JsonSerializer.Deserialize<TEvent>(json);
            await handle(message, ea);
            
            _channel.BasicAck(ea.DeliveryTag, multiple: false);
        };

        _channel.BasicConsume(queue, autoAck: false, consumer: consumer);
        
        return this;
    }
}