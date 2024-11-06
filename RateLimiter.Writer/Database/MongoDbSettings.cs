namespace RateLimiter.Writer.Database;

public class MongoDbSettings
{
    public string ConnectionString { get; set; }
    public string DatabaseName { get; set; }
    public string RateLimitCollectionName { get; set; }
}