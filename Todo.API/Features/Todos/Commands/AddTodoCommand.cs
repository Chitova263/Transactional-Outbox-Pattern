using MediatR;
using Todo.API.Infrastructure.Database;


namespace Todo.API.Features.Todos.Commands;

public class AddTodoCommand
{
    public class Command : IRequest<Unit>
    {
        public string Title { get; set; }
        public string Description { get; set; }
    }

    public class Handler : IRequestHandler<Command, Unit>
    {
        private readonly TodoDbContext _dbContext;
        private readonly ILogger<Handler> _logger;

        public Handler(TodoDbContext dbContext, ILogger<Handler> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        
        public async Task<Unit> Handle(Command command, CancellationToken cancellationToken)
        {
            var todo = Domain.Entities.Todo.Create(command.Title, command.Description);
            await _dbContext.AddAsync(todo, cancellationToken);
            await _dbContext.SaveEntitiesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}

