using MediatR;
using Todo.Feed.Events;
using Todo.Shared.Rabbitmq;

namespace Todo.Feed.Services;

public class EventBackgroundService : BackgroundService
{
    private readonly IEventSubscriber _eventSubscriber;
    private readonly ILogger<EventBackgroundService> _logger;
    private readonly IMediator _mediator;

    public EventBackgroundService(
        IEventSubscriber eventSubscriber,
        ILogger<EventBackgroundService> logger,
        IMediator mediator)
    {
        _eventSubscriber = eventSubscriber ?? throw new ArgumentNullException(nameof(eventSubscriber));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _eventSubscriber.Subscribe<TodoAddedEvent>(
            "Todos.TodoCreatedEvent",
            "TodoCreatedEvent",
            "Todos",
            async (message, args) =>
            {
                _logger.LogInformation("Received event: {EventId}, Message: {Message}", message.Id.ToString(),
                    message.Title);

                await _mediator.Publish(message);

                await Task.CompletedTask;
            }
        );
        return Task.CompletedTask;
    }
}