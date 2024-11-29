using MongoDB.Bson;
using MongoDB.Driver;
using RateLimiter.Reader.Models.DbModels;
using RateLimiter.Reader.Models.DomainModels;

namespace RateLimiter.Reader.Repository;

public interface IRateLimitRepository
{
    public IAsyncEnumerable<RateLimits> GetRateLimitsBatchAsync(int batchSize);

    public Task WatchRateLimitChangesAsync(Action<RateLimits> onRateLimitUpdate, Action<ObjectId> onRateLimitDelete);
}