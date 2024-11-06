using MongoDB.Driver;
using RateLimiter.Writer.Models.DbModels;

namespace RateLimiter.Writer.Database;

public class DatabaseInitializer
{
    private readonly IMongoDatabase _database;

    public DatabaseInitializer(MongoDbSettings settings)
    {
        var client = new MongoClient(settings.ConnectionString);
        _database = client.GetDatabase(settings.DatabaseName);
    }

    public IMongoCollection<RateLimitDbModel> GetRateLimitCollection()
    {
        return _database.GetCollection<RateLimitDbModel>("rate_limits");
    }
}