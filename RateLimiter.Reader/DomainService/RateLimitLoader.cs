using MongoDB.Driver;
using RateLimiter.Reader.Models.DbModels;
using RateLimiter.Reader.Models.mapper;
using RateLimiter.Reader.Repository;

namespace RateLimiter.Reader.DomainService;

public class RateLimitLoader
{
    private readonly IRateLimitRepository _rateLimitRepository;
    private readonly RateLimitMemoryStore _memoryStore;
    private readonly ILogger<RateLimitLoader> _logger;

    public RateLimitLoader(IRateLimitRepository rateLimitRepository, RateLimitMemoryStore memoryStore, ILogger<RateLimitLoader> logger)
    {
        _rateLimitRepository = rateLimitRepository;
        _memoryStore = memoryStore;
        _logger = logger;
    }
    public async Task LoadInitialDataAsync( int batchSize)
    {
        await foreach (var rateLimit in _rateLimitRepository.GetRateLimitsBatchAsync(batchSize))
        {
            _memoryStore.AddOrUpdateSingle(rateLimit);
            _logger.LogInformation($"Loaded rate limit for route: {rateLimit.Route}");
        }
    }
}