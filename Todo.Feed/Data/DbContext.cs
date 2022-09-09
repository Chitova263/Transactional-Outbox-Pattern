using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

namespace Todo.Feed.Data;

public class DbContext
{
    private readonly MongoClient _client;

    public DbContext(IOptions<MongoConfiguration> options)
    {
        _client = new MongoClient("mongodb://localhost:27017");
        Database = _client.GetDatabase("Todos");
        BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));

    }

    public IMongoDatabase Database { get; }

    public IMongoCollection<Entities.Feed> Feed => Database.GetCollection<Entities.Feed>("feed");
}