using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Todo.Domain;
using Todo.Domain.Entities;
using Todo.Domain.Shared;

namespace Todo.API.Infrastructure.Database;

public class TodoDbContext : DbContext, IUnitOfWork
{
    private readonly IMediator _mediator;
    public DbSet<Domain.Entities.Todo> Todos { get; set; }
    public DbSet<IntegrationEventOutBox> IntegrationEventOutBoxEvents { get; set; }
    
    public TodoDbContext(DbContextOptions<TodoDbContext> options, IMediator mediator)
     : base(options)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }
    
    public async Task SaveEntitiesAsync(CancellationToken cancellationToken = default)
    {
        // publish domain events
        // save integration events to transactional outbox
        // persist aggregate
        var domainEntities = ChangeTracker.Entries<Entity>()
            .Where(x => x.Entity.DomainEvents.Any());

        var domainEvents = domainEntities
            .SelectMany(x => x.Entity.DomainEvents)
            .ToList();
        
        // GET Integration Events
        var integrationEvents = domainEvents
            .Where(x => x is IIntegrationEvent)
            .Select(x => new IntegrationEventOutBox(x.GetType().Name, JsonConvert.SerializeObject(x)))
            .ToList();
        
        // Serialize The Integration Events And Persist In IntegrationOutbox Table
        await IntegrationEventOutBoxEvents.AddRangeAsync(integrationEvents, cancellationToken);

        await base.SaveChangesAsync(cancellationToken);
        domainEntities.ToList().ForEach(x => x.Entity.ClearDomainEvents());
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            .EnableSensitiveDataLogging()
            .LogTo(Console.WriteLine);
        base.OnConfiguring(optionsBuilder);
    }
}