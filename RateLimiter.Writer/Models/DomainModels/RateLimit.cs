using MongoDB.Bson;

namespace RateLimiter.Writer.Models.DomainModels;

public class RateLimit
{   
    public ObjectId Id { get; set; }
    public string Route { get; set; }
    public int RequestsPerMinute { get; set; }
}