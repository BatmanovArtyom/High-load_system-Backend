using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace RateLimiter.Reader.Models.DomainModels;

public class RateLimits
{
    [BsonId]
    public ObjectId Id { get; set; }
        
    [BsonElement("route")]
    public string Route { get; set; }
        
    [BsonElement("requests_per_minute")]
    public int RequestsPerMinute { get; set; }
}