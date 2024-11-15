using System.Collections.Concurrent;
using MongoDB.Bson;
using RateLimiter.Reader.Models.DbModels;

namespace RateLimiter.Reader.DomainService;

public class RateLimitMemoryStore
{
    private readonly ConcurrentDictionary<ObjectId, ReaderDbModel> _rateLimits = new();
    
    public void AddOrUpdatBatch(IEnumerable<ReaderDbModel> rateLimits)
    {
        foreach (var rateLimit in rateLimits)
        {
            _rateLimits[rateLimit.Id] = rateLimit;
        }
    }
    
    public void AddOrUpdateSingle(ReaderDbModel rateLimit)
    {
        _rateLimits[rateLimit.Id] = rateLimit;
    }
    
    public IEnumerable<ReaderDbModel> GetAllRateLimits() => _rateLimits.Values;
}