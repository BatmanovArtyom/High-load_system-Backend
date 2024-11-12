using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace RateLimiter.Reader.Models.DbModels;

public class ReaderDbModel
{
    [BsonId]
    public ObjectId Id { get; set; }
        
    [BsonElement("route")]
    public string Route { get; set; }
        
    [BsonElement("requests_per_minute")]
    public int RequestsPerMinute { get; set; }
}