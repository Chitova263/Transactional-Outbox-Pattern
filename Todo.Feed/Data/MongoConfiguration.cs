using Microsoft.Build.Framework;

namespace Todo.Feed.Data;

public class MongoConfiguration
{
    [Required] public string DatabaseName { get; set; }

    [Required] public string ConnectionString { get; set; }
}