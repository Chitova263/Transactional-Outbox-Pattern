using MediatR;
using Todo.Feed.Data;
using Todo.Feed.Services;
using Todo.Shared.Rabbitmq;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IEventSubscriber, EventSubscriber>();
builder.Services.AddHostedService<EventBackgroundService>();

builder.Services.Configure<MongoConfiguration>(builder.Configuration.GetSection("MongoSettings"));

builder.Services.AddSingleton<DbContext>();

builder.Services.AddMediatR(typeof(Program));

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