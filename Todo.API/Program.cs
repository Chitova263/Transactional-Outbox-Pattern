using MediatR;
using Microsoft.EntityFrameworkCore;
using Todo.API.Infrastructure.Database;
using Todo.API.Services;
using Todo.Shared.Rabbitmq;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMediatR(typeof(Program));

builder.Services
    .AddDbContext<TodoDbContext>(opts =>
    {
        opts.UseNpgsql(builder.Configuration.GetConnectionString("Database"));
    });

builder.Services.AddSingleton<TransactionalOutboxQueueBackgroundService>();
builder.Services.AddHostedService(provider => provider.GetService<TransactionalOutboxQueueBackgroundService>());
builder.Services.AddSingleton<IEventPublisher, EventPublisher>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();