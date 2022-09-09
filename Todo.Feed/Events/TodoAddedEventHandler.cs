using MediatR;
using Todo.Feed.Data;

namespace Todo.Feed.Events;

public class TodoAddedEventHandler : INotificationHandler<TodoAddedEvent>
{
    private readonly DbContext _dbContext;
    private readonly ILogger<TodoAddedEventHandler> _logger;

    public TodoAddedEventHandler(DbContext dbContext, ILogger<TodoAddedEventHandler> logger)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task Handle(TodoAddedEvent @event, CancellationToken cancellationToken)
    {
        _logger.LogInformation("TodoAddedEventHandler: {@event}", @event);

        var feed = new Data.Entities.Feed { Title = @event.Title, Description = @event.Description };
        await _dbContext.Feed.InsertOneAsync(feed, cancellationToken: cancellationToken);
    }
}