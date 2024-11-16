using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using RateLimiter.Reader.Models.DbModels;

namespace RateLimiter.Reader.DomainService;

public class RateLimitMemoryStore
{
    private readonly ConcurrentDictionary<ObjectId, ReaderDbModel> _rateLimits = new();
    private readonly ILogger<RateLimitMemoryStore> _logger;

    public RateLimitMemoryStore(ILogger<RateLimitMemoryStore> logger)
    {
        _logger = logger;
    }

    public void AddOrUpdatBatch(IEnumerable<ReaderDbModel> rateLimits)
    {
        foreach (var rateLimit in rateLimits)
        {
            _rateLimits[rateLimit.Id] = rateLimit;
            _logger.LogInformation("Added rate limit: Id={Id}, Route={Route}, RequestsPerMinute={RequestsPerMinute}",
                rateLimit.Id, rateLimit.Route, rateLimit.RequestsPerMinute);
        }
    }

    public void AddOrUpdateSingle(ReaderDbModel rateLimit)
    {
        _rateLimits[rateLimit.Id] = rateLimit;
        _logger.LogInformation("Added rate limit: Id={Id}, Route={Route}, RequestsPerMinute={RequestsPerMinute}",
            rateLimit.Id, rateLimit.Route, rateLimit.RequestsPerMinute);
    }
}