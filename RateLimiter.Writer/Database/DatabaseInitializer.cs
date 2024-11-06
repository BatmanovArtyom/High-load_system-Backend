using Microsoft.Extensions.Options;
using MongoDB.Driver;
using RateLimiter.Writer.Models.DbModels;

namespace RateLimiter.Writer.Database;

public class DatabaseInitializer
{
    private readonly IMongoDatabase _database;

    public DatabaseInitializer(IOptions<MongoDbSettings> settings)
    {
        var connectionString = settings.Value.MongoDb;
        var databaseName = settings.Value.DatabaseName;

        if (string.IsNullOrEmpty(connectionString))
        {
            throw new ArgumentNullException(nameof(connectionString), "Connection string cannot be null or empty.");
        }

        var client = new MongoClient(connectionString);
        _database = client.GetDatabase(databaseName);
    }

    public IMongoCollection<RateLimitDbModel> GetRateLimitCollection()
    {
        return _database.GetCollection<RateLimitDbModel>("rate_limits");
    }
}
