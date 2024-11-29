using Microsoft.Extensions.Options;
using MongoDB.Driver;
using RateLimiter.Reader.Models.DbModels;

namespace RateLimiter.Reader.Database;

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

    public IMongoCollection<ReaderDbModel> GetRateLimitCollection()
    {
        return _database.GetCollection<ReaderDbModel>("rate_limits");
    }
}