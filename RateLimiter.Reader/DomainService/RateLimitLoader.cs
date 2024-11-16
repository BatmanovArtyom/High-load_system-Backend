using MongoDB.Driver;
using RateLimiter.Reader.Models.DbModels;
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

    public async Task LoadInitialDataAsync( int BatchSize)
    {
        int skip = 0;

        while (true)
        {
            var batch = await _rateLimitRepository.GetRateLimitsBatchAsync(skip, BatchSize);
            if (batch.Count == 0) break;

            _memoryStore.AddOrUpdatBatch(batch);
            _logger.LogInformation($"Loaded {batch.Count} records to the database.");
            skip += BatchSize;
        }
    }
}