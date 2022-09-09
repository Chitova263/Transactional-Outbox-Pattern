using MongoDB.Bson.Serialization.Attributes;

namespace Todo.Feed.Data.Entities;

public class Feed
{
    [BsonId] 
    public Guid Id { get; set; }

    public string Description { get; set; }
    public string Title { get; set; }

    public Feed()
    {
        Id = Guid.NewGuid();
    }
}