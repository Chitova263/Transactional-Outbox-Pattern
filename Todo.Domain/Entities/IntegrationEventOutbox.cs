namespace Todo.Domain.Entities;

public class IntegrationEventOutBox
{
    public Guid Id { get; private set; }
    public string EventType { get; private set; }
    public string Payload { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? ProcessedAt { get; set; }

    public IntegrationEventOutBox(string eventType, string payload)
    {
        Id = Guid.NewGuid();
        EventType = eventType;
        Payload = payload;
        CreatedAt = DateTime.UtcNow;
    }
}
