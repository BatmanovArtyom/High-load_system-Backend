using MongoDB.Driver;
using RateLimiter.Reader.Models.DbModels;
using RateLimiter.Reader.Repository;

namespace RateLimiter.Reader.DomainService;

public class RateLimitLoader
{
    private const int BatchSize = 1000;
    private readonly IRateLimitRepository _rateLimitRepository;
    private readonly RateLimitMemoryStore _memoryStore;

    public RateLimitLoader(IRateLimitRepository rateLimitRepository, RateLimitMemoryStore memoryStore)
    {
        _rateLimitRepository = rateLimitRepository;
        _memoryStore = memoryStore;
    }

    public async Task LoadInitialDataAsync()
    {
        int skip = 0;

        while (true)
        {
            var batch = await _rateLimitRepository.GetRateLimitsBatchAsync(skip, BatchSize);
            if (batch.Count == 0) break;

            _memoryStore.AddOrUpdatBatch(batch);
            skip += BatchSize;
        }
    }
}