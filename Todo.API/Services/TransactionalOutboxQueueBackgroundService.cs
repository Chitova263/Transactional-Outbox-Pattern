using Todo.API.Infrastructure.Database;
using Todo.Shared.Rabbitmq;

namespace Todo.API.Services;

public class TransactionalOutboxQueueBackgroundService : BackgroundService
{
    private readonly ILogger<TransactionalOutboxQueueBackgroundService> _logger;
    private readonly IEventPublisher _eventPublisher;
    private readonly TodoDbContext _dbContext;

    public TransactionalOutboxQueueBackgroundService(ILogger<TransactionalOutboxQueueBackgroundService> logger, IServiceScopeFactory scopeFactory, IEventPublisher eventPublisher)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _eventPublisher = eventPublisher ?? throw new ArgumentNullException(nameof(eventPublisher));
        _dbContext = scopeFactory.CreateScope().ServiceProvider.GetService<TodoDbContext>() ?? throw new InvalidOperationException();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await PublishAsync(stoppingToken);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }

    private async Task PublishAsync(CancellationToken stoppingToken)
    {
        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var events = _dbContext.IntegrationEventOutBoxEvents
                    .Where(e => e.ProcessedAt == null)
                    .ToList();

                if (!events.Any())
                {
                    await Task.Delay(5000, stoppingToken);
                    continue;
                }

                foreach (var @event in events)
                {
                    await _eventPublisher.PublishAsync(
                        exchange: "Todos",
                        routingKey: @event.EventType,
                        message: @event.Payload,
                        messageId: @event.Id.ToString()
                    );
                    
                    @event.ProcessedAt = DateTime.UtcNow;
                    
                    await _dbContext.SaveChangesAsync(stoppingToken);
                    _logger.LogInformation("Published Event");
                }
                await Task.Delay(5000, stoppingToken);
            }
            
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            await Task.Delay(5000, stoppingToken);
        }
    }
}