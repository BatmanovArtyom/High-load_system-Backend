using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using RateLimiter.Reader.Models.DbModels;
using RateLimiter.Reader.Models.DomainModels;

namespace RateLimiter.Reader.DomainService;

public class RateLimitMemoryStore
{
    private readonly ConcurrentDictionary<ObjectId, RateLimits> _rateLimits = new();
    private readonly ILogger<RateLimitMemoryStore> _logger;

    public RateLimitMemoryStore(ILogger<RateLimitMemoryStore> logger)
    {
        _logger = logger;
    }
    
    public void AddOrUpdateSingle(RateLimits rateLimit)
    {
        _rateLimits[rateLimit.Id] = rateLimit;
        _logger.LogInformation("Added rate limit: Id={Id}, Route={Route}, RequestsPerMinute={RequestsPerMinute}",
            rateLimit.Id, rateLimit.Route, rateLimit.RequestsPerMinute);
    }
    
    public void RemoveById(ObjectId id)
    {
        if (_rateLimits.TryRemove(id, out var removed))
        {
            _logger.LogInformation($"Removed entry: {removed.Route}");
        }
    }
    
    public ICollection<RateLimits> GetAllRateLimits()
    {
        _logger.LogInformation("Fetching all rate limits from memory.");
        return _rateLimits.Values;
    }
}