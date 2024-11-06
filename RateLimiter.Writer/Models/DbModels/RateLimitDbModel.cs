﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace RateLimiter.Writer.Models.DbModels;

public class RateLimitDbModel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public int Id { get; set; }
    
    [BsonElement("route")]
    public string Route { get; set; }
    
    [BsonElement("requests_per_minute")]
    public int RequestsPerMinute { get; set; }
}